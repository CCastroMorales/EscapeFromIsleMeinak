using MeinakEsc.Components;
using MeinakEsc.GameObjects;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MeinakEsc
{
    public class Game : ParseCallback
    {
        public SceneHandler Scenes { get; set; } = new SceneHandler();
        public InputParser Parser { get; set; }
        public Inventory Inventory { get; set; } = new Inventory();
        public int InventoryLimit { get; set; } = 6;
        public bool Running { get; set; } = true;
        public int DefaultSleepMillis { get; set; } = 100;
        public bool FastForward { get; set; } = false;
        public ConsoleColor DefaultConsoleColor { get; } = Console.ForegroundColor;
        public Random random { get; } = new Random();
        protected int TextMarginLeft { get; set; } = 0;
        protected int TextMarginTop { get; set; } = 0;
        public bool Debug { get; set; } = false;
        private bool Restarted { get; set; } = false;

        public Game()
        {
            Parser = new InputParser(this);
            Console.Title = Strings.GAME_TITLE;
        }

        public void Run(string[] args)
        {
            Scenes = new SceneHandler();
            ClearInventory();
            
            ParseCommandLineArguments(args);
            Display.TitleScreen(Debug, Restarted, args);
            Scenes.LoadFirstScene();
            GameLoop();
        }

        private void ClearInventory()
        {
            Inventory = new Inventory();
        }

        public void ParseCommandLineArguments(string[] args)
        {
            int i = 0;
            if (args.Length > 0)
                foreach (string arg in args)
                {
                    if (arg == "+ff")
                        FastForward = true;
                    if (arg == "+debug")
                        Debug = true;
                    if (arg == "+jeepkey")
                        Dev.SpawnJeepKey(Inventory);
                    if (arg == "+note")
                        Dev.SpawnNote(Inventory);
                    if (arg == "+scene")
                    {
                        if (i <= args.Length - 2)
                        {
                            string sceneId = args[i + 1];
                            Id id;
                            Id.TryParse(sceneId, out id);

                            Scene scene = Scenes.LoadScene(id);
                            Scenes.Scenes.Remove(scene);
                            Scenes.Scenes.Insert(0, scene);
                        }
                    }
                    i++;
                }
        }

        private void GameLoop()
        {
            Running = true;

            while (Running)
            {
                Clear();
                PlayScene(Scenes.Active);
                ParseInput(Scenes.Active);
            }
        }

        private void ParseInput(Scene scene)
        {
            Parser.Reset();
            Parser.Inventory = Inventory;

            if (scene.AutoEnd)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;

                int x = TextMarginLeft;
                int y = Console.CursorTop;

                string text = Strings.PROMPT_ENTER_TO_CONTINUE;
                x = (Console.BufferWidth / 2) - (text.Length / 2);

                Thread.Sleep(Timing.EndScreenPressPromptDelay);
                Console.SetCursorPosition(x, y + 3);
                Console.WriteLine(text);

                var readkey = Console.ReadKey(true);
                while (readkey.Key != ConsoleKey.Enter)
                {
                    readkey = Console.ReadKey(true);
                }

                End();
                return;
            }

            while (!Parser.Done)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.CursorLeft = TextMarginLeft;
                int y = Console.CursorTop;
                Console.Write('»');
                Console.CursorVisible = true;
                string input = Console.ReadLine().Trim();
                Console.CursorVisible = false;

                if (input != "")
                {
                    Console.ForegroundColor = DefaultConsoleColor;

                    /*if (Debug && input.Split().Length == 2 && input.Split(' ')[0] == "debug.load")
                    {
                        string sceneId = input.Split(' ')[1];
                        LoadScene(sceneId);
                        return;
                    } else*/
                    Ctx context = new Ctx(this, Scenes.Active, Inventory);
                    Parser.Parse(context, input, scene);
                }
                else
                    Console.SetCursorPosition(Console.CursorLeft, y);
            }
        }

        private void PrintSceneTitle(Scene scene)
        {
            string[] words = scene.Title.Split(' ');

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.CursorVisible = false;
            Console.SetCursorPosition(TextMarginLeft, 0);
            foreach (string word in words)
            {
                Console.Write($"{word} ");
                Thread.Sleep(20);
                //Console.WriteLine(scene.Title);
            }
            Console.WriteLine("");
            Console.ForegroundColor = DefaultConsoleColor;
            Console.CursorVisible = true;
            Thread.Sleep(Timing.SleepTitleDuration);
        }

        private void GameOver()
        {
            Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Red;

            string gameOver = Strings.GAME_OVER.Replace(' ', '═');

            int x = (Console.BufferWidth / 2) - (gameOver.Length / 2);
            int y = (Console.WindowHeight / 3);

            Console.SetCursorPosition(0, y);

            for (int i = 0; i < x; i++)
            {
                Console.Write('═');
                Thread.Sleep(Timing.GameOverAnimationDuration);
            }

            for (int i = 0; i < gameOver.Length; i++)
            {
                Console.Write(gameOver[i]);
                Thread.Sleep(Timing.GameOverAnimationDuration);
            }

            for (int i = x + gameOver.Length; i < Console.BufferWidth; i++)
            {
                Console.Write('═');
                Thread.Sleep(Timing.GameOverAnimationDuration);
            }


            Console.ForegroundColor = ConsoleColor.DarkGray;
            
            string text = Strings.PROMPT_ENTER_TO_TRY_AGAIN;
            x = (Console.BufferWidth / 2) - (text.Length / 2);

            Thread.Sleep(Timing.EndScreenPressPromptDelay);
            Console.SetCursorPosition(x, y+3);
            Console.WriteLine(text);

            var readkey = Console.ReadKey(true);
            while (readkey.Key != ConsoleKey.Enter)
            {
                readkey = Console.ReadKey(true);
            }

            Console.CursorVisible = true;

            Scenes.RestorePreviousScene();
        }

        private void End()
        {
            Clear();
            Display.EndScreen();
            Clear();
            Running = false;
            Restarted = true;
            Run(new string[0]);
        }

        private void PlayScene(Scene scene)
        {
            PrintInventory(true);
            PrintCommandBox();
            PrintSceneTitle(scene);
            PrintSceneScript(scene);
        }

        public void PrintInventory(bool show)
        {
            // Allow the margins to be set even if we're not displaying the inventory.

            int invWidth = 15;
            TextMarginLeft = invWidth + 1;
            TextMarginTop = 2;

            if (!show)
                return;

            string title = "INVENTORY";
            bool evenPadding = (invWidth - title.Length - 2) % 2 == 0;
            int padding = (invWidth - title.Length - 2) / 2;

            Console.SetCursorPosition(0, 2);
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            /*for (int i = 0; i < invWidth; i++)
                Console.Write('═');
            Console.Write('\n');*/

            // Print title
            Console.Write('╔');

            for (int i = 0; i < padding; i++)
                Console.Write('═');

            for (int i = 0; i < title.Length; i++)
                Console.Write(title[i]);

            // Assumes 15 width
            for (int i = 0; i < padding - 1; i++)
                Console.Write('═');

            Console.Write('═');
            Console.Write('╗');
            Console.Write('\n');

            // Print items
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (Item item in Inventory.Items)
            {
                string label;

                if (item.Type == ItemType.WEAPON_FIREARM)
                    label = $"[{item.UsesRemaining}] {item.InventoryLabel}";
                else
                    label = item.InventoryLabel;

                padding = invWidth - label.Length - 3;
                
                Console.Write('║');
                Console.Write(' ');

                Console.ForegroundColor = DefaultConsoleColor;
                for (int i = 0; i < label.Length; i++)
                    Console.Write(label[i]);
                Console.ForegroundColor = ConsoleColor.DarkGray;

                for (int i = 0; i < padding; i++)
                    Console.Write(' ');

                Console.Write('║');
                Console.Write('\n');   
            }

            // Print empty lines
            int numEmptyLines = InventoryLimit - Inventory.Count;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            
            for (int i = 0; i < numEmptyLines; i++)
            {
                Console.Write('║');
                for (int j = 0; j < invWidth - 2; j++)
                    Console.Write('\u0000');
                Console.Write('║');
                Console.Write('\n');
            }

            // Bottom line
            for (int i = 0; i < invWidth; i++)
            {
                if (i == 0)
                    Console.Write('╚');
                else if (i == invWidth - 1)
                    Console.Write('╝');
                else
                    Console.Write('═');
            }
            Console.Write('\n');

            Console.ForegroundColor = DefaultConsoleColor;
            Console.CursorVisible = true;
        }

        private void PrintCommandBox()
        {
            Console.SetCursorPosition(0, 9);
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            int w = 15;
            int p = 0;
            string cmds = Strings.COMMANDS;

            for (int i = 0; i < w; i++)
                if (i == 0)
                    Console.Write('╠');
                else if (i == w - 1)
                    Console.Write('╣');
                else
                    Console.Write('═');

            Console.SetCursorPosition(3, 9);

            for (int i = 0; i < cmds.Length; i++)
                Console.Write(cmds[i]);


            // Print commands
            int y = 10;

            string[] commands = new string[] {
                "go",
                "use",
                "check",
                "take",
                "hit",
                "drop",
                "exit/quit"
            };

            foreach (string command in commands)
            {
                Console.SetCursorPosition(0, y);

                string cmd = $" {command.PadRight(w - 3)}";
                
                Console.Write('║');
                Console.Write(cmd);
                Console.Write('║');

                y++;
            }


            Console.SetCursorPosition(0, y);

            for (int i = 0; i < w; i++)
            {
                if (i == 0)
                    Console.Write('╚');
                else if (i == w - 1)
                    Console.Write('╝');
                else
                    Console.Write('═');
            }
            Console.Write('\n');
        }

        private void PrintSceneScript(Scene scene)
        {

            string[] scriptLines;

            if (scene.InitialVisit && scene.InitialScript.Count > 0)
                scriptLines = scene.InitialScript.ToArray();
            else
                scriptLines = scene.Script.ToArray();
            
            Console.CursorTop = TextMarginTop;
            PrintLines(scriptLines, Scenes.Active.Items.ToArray(), Scenes.Active.DroppedItems.ToArray(), Scenes.Active.Entities.ToArray());

            Scenes.Active.InitialVisit = false;
        }

        public void PrintLine(string line)
        {
            PrintLines(new string[] { line }, null, null, null);
        }

        public void PrintLine(string line, Item[] items)
        {
            PrintLines(new string[] { line }, items, null, null);
        }

        public void PrintLines(string[] lines)
        {
            PrintLines(lines, null, null, null);
        }

        public void PrintLines(string[] lines, Item[] items, Item[] dropped, Entity[] entities)
        {
            Console.CursorVisible = false;
            Console.CursorLeft = TextMarginLeft;

            // Last minute hack.
            List<string> hackyLines = new List<string>();
            hackyLines.AddRange(lines);

            if (dropped != null && dropped.Length > 0)
            {
                string droppedItemsText = "Dropped items: ";
                foreach (Item item in dropped)
                {
                    droppedItemsText += item.Name;

                    // is not last item...
                    if (dropped[dropped.Length - 1] != item)
                        droppedItemsText += ", ";                
                }

                hackyLines.Add("");
                hackyLines.Add(droppedItemsText);
            }

            List<char> punctuation = new List<char> { ' ', '.', ';', ',', '?', '!', '\"', '\'', '-' };

            // Go through all the lines and print them. Replace special tokens with data.
            foreach (string line in hackyLines)
            {
                string output = line;

                if (items != null)
                    output = InsertItemDescriptions(items, output);
                if (entities != null)
                    output = InsertEntityDescriptions(entities, output);
                
                output = output.Replace("\r", "");
                output = output.Replace("  ", " ");

                string[] words = output.Split(' ');

                foreach (string word in words)
                {
                    ConsoleColor wordColor;

                    if (IsCapitalizedKeyword(word, punctuation.ToArray()))
                        wordColor = ConsoleColor.Blue;
                    else
                        wordColor = DefaultConsoleColor;

                    int x = Console.CursorLeft;
                    int y = Console.CursorTop;
                    int length = x + word.Length + 1;

                    if (length >= Console.BufferWidth)
                    {
                        x = TextMarginLeft;
                        y += 1;
                        Console.SetCursorPosition(x, y);
                    }

                    if (word.Length > 0)
                    {
                        foreach (char c in word)
                        {
                            if (punctuation.Contains(c))
                                Console.ForegroundColor = DefaultConsoleColor;
                            else
                                Console.ForegroundColor = wordColor;

                            Console.Write(c);

                            if (punctuation.Contains(c))
                            {
                                if (!FastForward)
                                    Thread.Sleep(Timing.PunctuationDuration);
                            }
                            else
                            {
                                if (!FastForward)
                                    Thread.Sleep(Timing.SleepCharDuration);
                            }
                        }
                    }

                    Console.Write(' ');
                }

                Console.Write("\n");
                Console.SetCursorPosition(TextMarginLeft, Console.CursorTop);
                Thread.Sleep(DefaultSleepMillis);
            }
        }

        public string InsertItemDescriptions(Item[] items, string text)
        {
            string descriptions = "";

            foreach (Item item in items)
                descriptions += item.Description + " ";

            return text.Replace("ITEM_DESCRIPTIONS", descriptions.Trim()); ;
        }

        public string InsertEntityDescriptions(Entity[] entities, string text)
        {
            string descriptions = "";
            Func<Entity, string> addEntityDescription = x => descriptions += x.Description + " ";

            foreach (Entity entity in entities)
            {
                if (entity.Dead)
                {
                    if (entity.ShowDescriptionWhenDead)
                        addEntityDescription(entity);
                }
                else if (entity.Passive)
                {
                    if (entity.ShowDescriptionWhenPassive)
                        addEntityDescription(entity);
                }
                else
                    addEntityDescription(entity);
            }

            return text.Replace("ENTITY_DESCRIPTIONS", descriptions.Trim());
        }

        private bool IsCapitalizedKeyword(string wordWithPunctuation, char[] symbols)
        {
            string word = wordWithPunctuation;

            // Probably could've regex'd this.
            foreach (char symbol in symbols)
                word = word.Replace($"{symbol}","");

            if (word == "I")
                return false;

            for (int i = 0; i < word.Length; i++)
                if (char.IsLetter(word[i]) && !char.IsUpper(word[i]))
                    return false;
            return true;
        }

        /*private void PresentOptions(SceneV1 scene)
        {
            foreach (Option option in scene.Options)
            {
                Console.WriteLine(option.text);
            }
        }*/

        public void Print(string text)
        {
            int x = TextMarginLeft;
            int y = Console.CursorTop;

            Console.SetCursorPosition(x, y);

            string[] words = text.Split(' ');
            foreach (string word in words)
            {
                x = Console.CursorLeft;
                y = Console.CursorTop;

                if (x + word.Length + 1 > Console.BufferWidth)
                {
                    x = TextMarginLeft;
                    y += 1;
                    Console.SetCursorPosition(x, y);
                }

                foreach (Char c in word)
                {
                    Console.Write(c);

                    if (c == '.' || c == ',' || c == ';' || c == ':' || c == '?' || c == '"')
                    {
                        if (!FastForward)
                            Thread.Sleep(Timing.PunctuationDuration);
                    }
                    else
                    {
                        if (!FastForward)
                            Thread.Sleep(Timing.SleepCharDuration);
                    }
                }

                Console.Write(' ');
            }
        }

        private void PrintEnterToContinue()
        {
            int x = TextMarginLeft;
            int y = Console.CursorTop;

            Console.CursorVisible = false;
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            string text = Strings.PROMPT_ENTER_TO_CONTINUE;
            Console.Write(text);
            
            var readkey = Console.ReadKey(true);
            while (readkey.Key != ConsoleKey.Enter)
            {
                readkey = Console.ReadKey(true);
            }
            Console.ForegroundColor = DefaultConsoleColor;
            Console.CursorVisible = true;
        }

        private void Clear()
        {
            Console.CursorVisible = false;
            //Console.Clear();
            for (int y = 0; y < Console.WindowHeight; y++)
            {
                //Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.SetCursorPosition(0, y);

                for (int x = 0; x < Console.WindowWidth; x++)
                {
                    Console.Write(' ');
                }
                Console.Write('\n');

                //Console.WriteLine($"new line of text {y}");
                Thread.Sleep(1);
            }
            Console.Clear();
            Console.CursorVisible = true;
        }

        /* Callbacks from InputParser */

        public void OnPrint(string text)
        {
            PrintLine(text);
        }

        public bool HasPreviousScene()
        {
            return Scenes.Previous != null;
        }

        public void OnPreviousScene()
        {
            Scenes.LoadPreviousScene();
        }

        public void OnExitScene(Exit exit)
        {
            if (!TriggerEntity(exit.TriggerEntityId))
            {
                Scene loadedScene = Scenes.LoadScene(exit.Destination);

                if (loadedScene == null)
                    throw new Exception($"There is no loaded scene (exit:[\"{exit.Commands}\":{exit.Destination}])");
            }
        }

        public bool TriggerEntity(Id triggerEntityId)
        {
            Entity entity = Scenes.Active.FindEntity(triggerEntityId);

            if (entity == null || entity.Passive)
                return false;

            PrintLine(entity.TriggerDescription);
            PrintEnterToContinue();

            if (entity.TriggerGameOver)
                GameOver();

            return true;
        }

        public void OnTakeItem(Item item, string label)
        {
            if (Inventory.Count == InventoryLimit)
            {
                PrintLine("You can't take the item; your inventory is full.");
                return;
            }

            Scene scene = Scenes.Active;

            scene.RemoveItem(item);
            Inventory.Add(item);

            string text = $"You take the {label.ToLower()}.";
            PrintLine(text);

            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            PrintInventory(true);
            PrintCommandBox();
            Console.SetCursorPosition(x, y);
        }

        public void OnReadItem(Item item, ItemAction action, string label)
        {
            if (action == null)
                PrintLine("You can't do that!");
            else
                PrintLine(action.Text);
        }

        private bool KillEntityWith(Entity entity, ItemType itemType)
        {
            if (entity.KillBy.Contains(itemType))
            {
                PrintLine(entity.KilledDescription);
                Item drop = entity.Kill();
                Scenes.Active.Entities.Remove(entity);

                if (drop != null)
                {
                    PrintLine(drop.Description);
                    Scenes.Active.Items.Add(drop);
                }

                return true;
            }

            return false;
        }

        public void OnPunchEntity(Entity entity, string entityName)
        {
            if (entity is Human && entity.MaxKillAttempts > 0)
            {
                entity.KillAttempt++;

                string description = entity.KillFailDescriptions[0];
                entity.KillFailDescriptions.RemoveAt(0);
                PrintLine(description);

                if (entity.KillAttempt > entity.MaxKillAttempts)
                {
                    Parser.Done = true; // hacky.
                    PrintEnterToContinue();
                    GameOver();
                    return;
                }
            }
            else if (KillEntityWith(entity, ItemType.FIST))
                return;
            else
                PrintLine($"You tried to punch {entity.Name.ToLower()} but hit nothing but air.");
        }

        public void OnUse(Item item, string itemName, string target)
        {
            Scene scene = Scenes.Active;

            // Check if target is an entity
            Entity entity = scene.FindEntity(target);

            if (entity != null)
            {
                if (KillEntityWith(entity, item.Type))
                {
                    ItemUsed(item, false);
                    return;
                }
                else if (entity.PacifyWith.Contains(item.Type))
                {
                    PrintLine(entity.PacifyDescription);
                    entity.Passify();

                    ItemUsed(item, false);
                    return;
                }

                PrintLine("Huh?");
                ItemUsed(item, false);
                return;
            }
            else if (target == "VEHICLE_JEEP")
            {
                // Hack: transfer glove compartment.
                var gloveCompartment = Scenes.Active.FindCheckObject(Id.CHECK_OBJECT_COMPARTMENT);
                Id nextSceneId = Id.SCENE_SPECIAL_VEHICLE_JEEP_DRIVING;
                Scene drivingScene = Scenes.LoadScene(nextSceneId);
                drivingScene.Objects.Add(gloveCompartment);
                
                Scenes.Previous = null;
                ItemUsed(item, true);
                return;
            }
            else if (target == "VEHICLE_BOAT_46")
            {
                Scenes.LoadScene(Id.SCENE_SPECIAL_VEHICLE_BOAT_DRIVING);
                Scenes.Previous = null;
                ItemUsed(item, true);
                return;
            }

            PrintLine($"You can't use {item.LowerCaseName} like that!");
        }

        private void ItemUsed(Item item, bool silenceLoseDescription)
        {
            item.Use();

            if (item.UsesRemaining == 0 && item.LoseOnNoRemainingUses)
                LoseItem(item, silenceLoseDescription);
            else if (item.LoseOnUse)
                LoseItem(item, silenceLoseDescription);

            UpdateInventoryScreen();
        }

        private void LoseItem(Item item, bool silenceLoseDescription)
        {
            Inventory.Remove(item);
            if (!silenceLoseDescription)
                PrintLine($"You lose the {item.LowerCaseName}.");
            UpdateInventoryScreen();
        }

        public void UpdateInventoryScreen()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            PrintInventory(true);
            PrintCommandBox();
            Console.SetCursorPosition(x, y);
        }
    }
}
