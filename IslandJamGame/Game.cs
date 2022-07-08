using Newtonsoft.Json;
using ScriptLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace IslandJamGame
{
    public class Game : ParseCallback
    {
        public InputParser Parser { get; set; }
        public Script Script { get; set; }
        public List<Item> Inventory { get; set; } = new List<Item>();
        public int InventoryLimit { get; set; } = 6;
        public Scene PreviousScene { get; set; }
        public bool Running { get; set; } = true;
        public int DefaultSleepMillis { get; set; } = 100;
        public Scene Scene { get; set; }
        public bool FastForward { get; set; } = false;
        public ConsoleColor DefaultConsoleColor { get; } = Console.ForegroundColor;
        public Random random { get; } = new Random();
        protected int TextMarginLeft { get; set; } = 0;
        protected int TextMarginTop { get; set; } = 0;
        public bool Debug { get; set; } = false;

        public Game(string[] args)
        {
            ParseCommandLineArguments(args);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            LoadData();

            Parser = new InputParser(this);
        }

        private void ParseCommandLineArguments(string[] args)
        {
            if (args.Length > 0)
                foreach (string arg in args)
                {
                    if (arg == "+ff")
                        FastForward = true;
                    if (arg == "+debug")
                        Debug = true;
                }
        }

        private void LoadData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string[] names = assembly.GetManifestResourceNames();
            using (var stream = assembly.GetManifestResourceStream("IslandJamGame.res.script.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    Script = JsonConvert.DeserializeObject<Script>(json);
                }
            }

            //string json = File.ReadAllText("res/script/script.json");
        }

        // https://denhamcoder.net/2018/08/25/embedding-net-assemblies-inside-net-assemblies/
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("IslandJamGame.res.Newtonsoft.Json.dll"))
            {
                var assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

        public void Run()
        {
            Scene = Script.Scenes[0];

            //Console.WriteLine($"{Console.WindowWidth}");
            //Console.WriteLine($"{Console.WindowHeight}");

            while (Running)
            {
                Clear();
                PlayScene(Scene);
                PresentOptions(Scene);
                ParseInput(Scene);
            }

        }

        private void ParseInput(Scene scene)
        {
            Parser.Reset();
            Parser.Inventory = Inventory;
            bool requireInput = true;

            while (!Parser.Done)
            {

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.CursorLeft = TextMarginLeft;
                Console.Write('»');
                string input = Console.ReadLine().Trim();
                Console.ForegroundColor = DefaultConsoleColor;

                if (Debug && input.Split().Length == 2 && input.Split(' ')[0] == "debug.load")
                {
                    string sceneId = input.Split(' ')[1];
                    LoadScene(sceneId);
                    return;
                } else
                    Parser.Parse(input, scene);
                
                requireInput = false;
            }

            /*if (!scene.HasOptions)
            {
                Console.ReadLine();

                if (scene.NextScene == "GAME_OVER")
                    GameOver();
                else
                    LoadNextScene(scene.NextScene);
                return;
            }*/

            while (requireInput)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("> ");
                string input = Console.ReadLine().Trim();
                Console.ForegroundColor = DefaultConsoleColor;

                string[] args = input.Split(' ');

                for (int i = 0; i < args.Length; i++)
                    args[i] = args[i].Trim();
                
                string cmd;
                string argument = "";


                if (args.Length == 1)
                    cmd = input.ToLower();
                else
                {
                    cmd = args[0];
                    argument = args[1];
                }


                if (cmd == Commands.OPEN)
                {
                    if (args.Length <= 1)
                        Console.WriteLine("Open what?");
                    else if (Scene.HasContainer(argument))
                    {
                        Container container = scene.Container(argument);
                        foreach (string item in container.Items)
                            Console.WriteLine(item);
                        return;
                    }
                }

                /*else if (scene.HasOptions)
                    foreach (Option option in Scene.Options)
                        if (cmd != "" && option.Command == cmd)
                        {
                            LoadNextScene(option.NextScene);
                            validInput = true;
                            return;
                        }*/
                else
                    Console.WriteLine(">>> Error: Command expected.");
            }
        }

        private void PrintCheckDescription(Scene scene, InteractiveObject interactiveObject)
        {
            //Clear();
            //PrintSceneTitle(scene);
            PrintText(interactiveObject.Text);
        }

        private void PrintSceneTitle(Scene scene)
        {
            string[] words = scene.Title.Split(' ');

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(TextMarginLeft, 0);
            foreach (string word in words)
            {
                Console.Write($"{word} ");
                Thread.Sleep(20);
                //Console.WriteLine(scene.Title);
            }
            Console.WriteLine("");
            Console.ForegroundColor = DefaultConsoleColor;
            Thread.Sleep(Timing.SleepTitleDuration);
        }

        private void GameOver()
        {
            Clear();
            Console.WriteLine("G A M E    O V E R");
            
            Thread.Sleep(2000);

            Scene = Script.Scenes[0];
        }

        private void PlayScene(Scene scene)
        {
            PrintInventory(true);
            PrintSceneTitle(scene);
            PrintText(scene.text);
        }

        private void PrintInventory(bool show)
        {
            // Allow the margins to be set even if we're not displaying the inventory.

            int invWidth = 15;
            int ih = 10;
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
                padding = invWidth - item.Name.Length - 3;
                
                Console.Write('║');
                Console.Write(' ');

                Console.ForegroundColor = DefaultConsoleColor;
                for (int i = 0; i < item.Name.Length; i++)
                    Console.Write(item.Name[i]);
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

        private void PrintText(List<string> lines)
        {
            PrintText(lines.ToArray());
        }

        private void PrintText(string[] lines)
        {
            Console.CursorLeft = TextMarginLeft;
            Console.CursorTop = TextMarginTop;

            foreach (string originalText in lines)
            {
                string textWithItemDesc = InsertItemDescriptions(Scene, originalText);
                string text = InsertEntityDescriptions(Scene, textWithItemDesc);
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
                    }

                    Console.SetCursorPosition(x, y);

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
                Console.WriteLine("");
                Console.SetCursorPosition(TextMarginLeft, Console.CursorTop);
                Thread.Sleep(DefaultSleepMillis);
            }
        }

        private string InsertItemDescriptions(Scene scene, string text)
        {
            string descriptions = "";

            foreach (Item item in Scene.Items)
                descriptions += item.Description + " ";

            return text.Replace("ITEM_DESCRIPTIONS", descriptions.Trim()); ;
        }

        private string InsertEntityDescriptions(Scene scene, string text)
        {
            string descriptions = "";

            foreach (Entity entity in scene.Entities)
            {
                if (entity.Dead)
                {
                    if (entity.ShowDescriptionWhenDead)
                        descriptions += entity.Description + " ";
                }
                else
                    descriptions += entity.Description + " ";
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

        private void PresentOptions(Scene scene)
        {
            foreach (Option option in scene.Options)
            {
                Console.WriteLine(option.text);
            }
        }

        private void PrintLine(string text)
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

            Console.Write('\n');
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

        private void LoadScene(string sceneId)
        {
            foreach (Scene scene in Script.Scenes)
                if (scene.SceneId == sceneId)
                {
                    PreviousScene = Scene;
                    Scene = scene;
                    return;
                }
            Scene = null;
        }

        /* Callbacks from InputParser */

        public void OnPrint(string text)
        {
            PrintLine(text);
        }

        public bool HasPreviousScene()
        {
            return PreviousScene != null;
        }

        public void OnPreviousScene()
        {
            Scene currentScene = Scene;
            Scene = PreviousScene;
            PreviousScene = currentScene;
        }

        public void OnExitScene(string sceneId, string exit)
        {
            LoadScene(sceneId);
        }

        public void OnCheckDescription(string objectName)
        {
            InteractiveObject interactiveObject = Scene.ObjectbyName(objectName);
            PrintCheckDescription(Scene, interactiveObject);
        }

        public void OnTakeItem(string itemLabel)
        {
            if (Inventory.Count == InventoryLimit)
            {
                PrintLine("You can't take the item; your inventory is full.");
                return;
            }

            Item item = Scene.Take(itemLabel);
            Inventory.Add(item);

            string text = $"You take the {itemLabel.ToLower()}.";
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

        public void OnPunchEntity(string entityName)
        {
            Entity entity = Scene.EntityByName(entityName);

            if (entity.KillBy.Contains("TYPE_MELEE"))
            {
                Item drop = entity.Kill();

                if (drop != null)
                {
                    PrintLine(drop.Description);
                    Scene.Items.Add(drop);
                }
            } 
            else
            {
                PrintLine($"You tried to punch {entity.Name} but hit nothing but air.");
            }
        }
    }
}
