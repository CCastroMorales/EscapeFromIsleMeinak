using IslandJamGame.Engine;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IslandJamGame
{
    public class Game : ParseCallback
    {
        public SceneHandler Scenes { get; } = new SceneHandler();
        public InputParser Parser { get; set; }
        public List<Item> Inventory { get; set; } = new List<Item>();
        public int InventoryLimit { get; set; } = 6;
        public bool Running { get; set; } = true;
        public int DefaultSleepMillis { get; set; } = 100;
        public bool FastForward { get; set; } = false;
        public ConsoleColor DefaultConsoleColor { get; } = Console.ForegroundColor;
        public Random random { get; } = new Random();
        protected int TextMarginLeft { get; set; } = 0;
        protected int TextMarginTop { get; set; } = 0;
        public bool Debug { get; set; } = false;

        public Game()
        {
            Parser = new InputParser(this);
            Console.Title = Strings.GAME_TITLE;
        }

        public void Run(string[] args)
        {
            TitleScreen.Display(Debug, args);
            Scenes.LoadFirstScene();
            GameLoop();
        }

        private void GameLoop()
        {
            while (Running)
            {
                Clear();
                PlayScene(Scenes.Active);
                //PresentOptions(scene);
                ParseInput(Scenes.Active);
            }
        }

        private void ParseInput(Scene scene)
        {
            Parser.Reset();
            Parser.Inventory = Inventory;

            while (!Parser.Done)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.CursorLeft = TextMarginLeft;
                int y = Console.CursorTop;
                Console.Write('»');
                string input = Console.ReadLine().Trim();

                if (input != "")
                {
                    Console.ForegroundColor = DefaultConsoleColor;

                    /*if (Debug && input.Split().Length == 2 && input.Split(' ')[0] == "debug.load")
                    {
                        string sceneId = input.Split(' ')[1];
                        LoadScene(sceneId);
                        return;
                    } else*/
                    Parser.Parse(input, scene);
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

            Thread.Sleep(Timing.GameOverPressPromptDelay);
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

        private void PlayScene(Scene scene)
        {
            PrintInventory(true);
            PrintSceneTitle(scene);
            PrintSceneScript(scene);
        }

        private void PrintInventory(bool show)
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
            foreach (Item item in Inventory)
            {
                string label = item.InventoryLabel;
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

        private void PrintSceneScript(Scene scene)
        {
            Console.CursorVisible = false;
            Console.CursorLeft = TextMarginLeft;
            Console.CursorTop = TextMarginTop;

            string[] scriptLines;

            if (scene.InitialVisit && scene.InitialScript.Count > 0)
                scriptLines = scene.InitialScript.ToArray();
            else
                scriptLines = scene.Script.ToArray();


            foreach (string originalText in scriptLines)
            {
                string textWithItemDesc = InsertItemDescriptions(Scenes.Active, originalText);
                string text = InsertEntityDescriptions(Scenes.Active, textWithItemDesc);
                text = text.Replace("\r","");
                text = text.Replace("  ", " ");

                string[] words = text.Split(' ');

                foreach (string word in words)
                {
                    if (IsCapitalized(word))
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = DefaultConsoleColor;

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
                    }

                    Console.Write(' ');
                }

                Console.Write("\n");
                Console.SetCursorPosition(TextMarginLeft, Console.CursorTop);
                Thread.Sleep(DefaultSleepMillis);
            }
            Console.CursorVisible = true;

            Scenes.Active.InitialVisit = false;
        }

        private string InsertItemDescriptions(Scene scene, string text)
        {
            string descriptions = "";

            foreach (Item item in scene.Items)
                descriptions += item.Description + " ";

            return text.Replace("ITEM_DESCRIPTIONS", descriptions.Trim()); ;
        }

        private string InsertEntityDescriptions(Scene scene, string text)
        {
            string descriptions = "";
            Func<Entity, string> addEntityDescription = x => descriptions += x.Description + " ";

            foreach (Entity entity in scene.Entities)
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

        private bool IsCapitalized(string word)
        {
            for (int i = 0; i < word.Length; i++)
                if (Char.IsLetter(word[i]) && !Char.IsUpper(word[i]))
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

        private void PrintLine(string text)
        {
            Print(text);
            Console.Write('\n');
        }

        private void Print(string text)
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

        public void OnCheckDescription(string objectName)
        {
            /*InteractiveObject interactiveObject = Scene.ObjectbyName(objectName);
            PrintCheckDescription(Scene, interactiveObject);*/
        }

        public void OnTakeItem(Item item, string label)
        {
            if (Inventory.Count == InventoryLimit)
            {
                PrintLine("You can't take the item; your inventory is full.");
                return;
            }

            Scene scene = Scenes.Active;
            scene.Items.Remove(item);
            Inventory.Add(item);

            string text = $"You take the {label.ToLower()}.";
            PrintLine(text);

            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            PrintInventory(true);
            Console.SetCursorPosition(x, y);
        }

        public void OnReadItem(Item item, ItemAction action, string label)
        {
            if (action == null)
                PrintLine("You can't do that!");
            else
                PrintLine(action.Text);
        }

        public void OnPunchEntity(Entity entity, string entityName)
        {
            // Consider the punch a melee "item".
            if (entity.KillBy.Contains(ItemType.MELEE))
            {
                Scene scene = Scenes.Active;

                Item drop = entity.Kill();
                scene.Entities.Remove(entity);

                if (drop != null)
                {
                    PrintLine(drop.Description);
                    scene.Items.Add(drop);
                }
            } 
            else
            {
                PrintLine($"You tried to punch {entity.Name} but hit nothing but air.");
            }
        }

        public void OnUse(Item item, string itemName, string target)
        {
            Scene scene = Scenes.Active;

            // Check if target is an entity
            Entity entity = scene.FindEntity(target);

            if (entity != null)
            {
                if (entity.KillBy.Contains(item.Type))
                {
                    PrintLine(entity.KilledDescription);
                    Item drop = entity.Kill();
                    // TODO: Check if remove?
                    scene.Entities.Remove(entity);

                    if (drop != null)
                    {
                        PrintLine(drop.Description);
                        scene.Items.Add(drop);
                    }
                }
                else if (entity.PassifyWith.Contains(item.Type))
                {
                    PrintLine(entity.PassifyDescription);
                    entity.Passify();
                }

                LoseItem(item, false);
                return;
            }
            else if (target == "VEHICLE_JEEP")
            {
                Scenes.LoadScene(Id.SCENE_SPECIAL_VEHICLE_JEEP_DRIVING);
                Scenes.Previous = null;
                LoseItem(item, true);
                return;
            }

            PrintLine($"You can't use {item.LowerCaseName} like that!");
        }

        private void LoseItem(Item item, bool silent)
        {
            if (item.LoseOnUse)
            {
                Inventory.Remove(item);
                if (!silent)
                    PrintLine($"You lose the {item.LowerCaseName}.");

                int x = Console.CursorLeft;
                int y = Console.CursorTop;

                PrintInventory(true);
                Console.SetCursorPosition(x, y);
            }
        }
    }
}
