using Newtonsoft.Json;
using ScriptLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace IslandJamGame
{
    public class Game
    {
        public Script Script { get; set; }
        public string[] Inventory { get; set; } = new string[6];
        public Scene PreviousScene { get; set; }
        public bool Running { get; set; } = true;
        public int DefaultSleepMillis { get; set; } = 100;
        public int DefaultTitleSleepMillis { get; set; } = 1000;
        public Scene Scene { get; set; }
        public bool FastForward { get; set; } = false;
        public ConsoleColor DefaultConsoleColor { get; } = Console.ForegroundColor;
        public Random random { get; } = new Random();
        protected int TextPos { get; set; } = 0;
        protected int TextPosY { get; set; } = 0;

        public Game(string[] args)
        {
            ParseCommandLineArguments(args);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            LoadData();
        }

        private void ParseCommandLineArguments(string[] args)
        {
            if (args.Length > 0)
                foreach (string arg in args)
                {
                    if (arg == "+ff")
                        FastForward = true;
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
            bool requireInput = true;
            bool validInput = false;

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


                if (cmd == Commands.GO)
                {
                    if (args.Length <= 1)
                        Console.WriteLine("Go where?");
                    if (argument.ToLower() == Arguments.BACK)
                    {
                        if (PreviousScene == null)
                            Console.WriteLine("There's nowhere to go back to.");
                        else
                        {
                            LoadBackScene();
                            requireInput = false;
                        }
                    }
                    else if (Scene.HasExit(argument))
                    {
                        LoadNextScene(scene.SceneFromExit(argument));
                        validInput = true;
                        requireInput = false;
                    }
                    else
                        Console.WriteLine("Unknown location.");
                }

                else if (cmd == Commands.CHECK)
                {
                    if (args.Length <= 1)
                    {
                        Console.Write("Check what?\n");
                        Console.WriteLine("");
                    }
                    else
                    {
                        if (scene.HasInteractiveObject(argument))
                        {
                            InteractiveObject interactiveObject = scene.ObjectbyName(argument);
                            PrintCheckDescription(scene, interactiveObject);
                        }
                    }
                }

                else if (cmd == Commands.OPEN)
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

                else if (scene.HasOptions)
                    foreach (Option option in Scene.Options)
                        if (cmd != "" && option.Command == cmd)
                        {
                            LoadNextScene(option.NextScene);
                            validInput = true;
                            return;
                        }
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
            /*Console.WriteLine($"{Console.BufferWidth}");

            int x = 0;

            for (int i = 0; i < Console.BufferWidth; i++)
            {
                Console.Write(x);
                Thread.Sleep(10);

                x++;

                if (x >= 10)
                    x = 0;
            }*/

            string[] words = scene.Title.Split(' ');

            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (string word in words)
            {
                Console.Write($"{word} ");
                Thread.Sleep(20);
                //Console.WriteLine(scene.Title);
            }
            Console.WriteLine("");
            Console.ForegroundColor = DefaultConsoleColor;
            Thread.Sleep(DefaultTitleSleepMillis);
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
            PrintSceneTitle(scene);
            PrintInventory();
            PrintText(scene.text);
        }

        private void PrintInventory()
        {
            int iw = 13;
            int ih = 10;
            TextPos = iw + 1;
            TextPosY = 2;

            string title = "INVENTORY";

            for (int i = 0; i < iw; i++)
                Console.Write('=');

            Console.Write('\n');
            Console.Write('|');
            Console.Write(' ');

            for (int i = 0; i < title.Length; i++)
                Console.Write(title[i]);

            Console.Write(' ');
            Console.Write('|');
            Console.Write('\n');

            for (int i = 0; i < Inventory.Length; i++)
            {
                Console.Write('|');
                for (int j = 0; j < iw - 2; j++)
                    Console.Write('\u0000');
                    //Console.Write(' ');
                Console.Write('|');
                Console.Write('\n');
            }

            for (int i = 0; i < iw; i++)
                Console.Write('=');
            Console.Write('\n');
        }

        private void PrintText(List<string> lines)
        {
            PrintText(lines.ToArray());
        }

        private void PrintText(string[] lines)
        {
            Console.CursorLeft = TextPos;
            Console.CursorTop = TextPosY;
            int pos = TextPos;

            foreach (string text in lines)
            {
                string[] words = text.Split(' ');

                foreach (string word in words)
                {
                    if (IsCapitalized(word))
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = DefaultConsoleColor;

                    /*foreach (Char c in word)
                    {
                        Console.Write(c);

                        if (c == '.' || c == ',' || c == ';' || c == ':' || c == '?' || c == '"')
                        {
                            if (!FastForward)
                                Thread.Sleep(random.Next(500,750));
                        }
                        else
                        {
                            if (!FastForward)
                            Thread.Sleep(10);
                        }
                    }*/

                    int x = Console.CursorLeft;
                    int y = Console.CursorTop;

                    //Console.SetCursorPosition(0, 0);
                    //Console.WriteLine($"{Console.BufferWidth}:{pos} {x},{y}                         ");

                    pos += word.Length + ' ';

                    if (x + word.Length + 1 > Console.BufferWidth)
                    {
                        x = TextPos;
                        y += 1;
                    }

                    Console.SetCursorPosition(x, y);
                    //Console.Write(word);

                    if (word.Length > 0)
                    {
                        foreach (Char c in word)
                        {
                            Console.Write(c);

                            //Char lastChar = word[word.Length - 1];

                            if (c == '.' || c == ',' || c == ';' || c == ':' || c == '?' || c == '"')
                            {
                                if (!FastForward)
                                    Thread.Sleep(random.Next(400, 600));
                            }
                            else
                            {
                                if (!FastForward)
                                    Thread.Sleep(12);
                            }
                        }
                    }

                    Console.Write(' ');
                    //Console.Write("\n");
                    //Console.WriteLine("");
                }

                //Console.WriteLine(text);
                Console.Write("\n");
                Console.WriteLine("");
                Console.SetCursorPosition(TextPos, Console.CursorTop);
                Thread.Sleep(DefaultSleepMillis);
            }
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

        private void LoadNextScene(string sceneName)
        {
            foreach (Scene scene in Script.Scenes)
                if (scene.SceneId == sceneName)
                {
                    PreviousScene = Scene;
                    Scene = scene;
                    return;
                }
            Scene = null;
        }

        private void LoadBackScene()
        {
            Scene currentScene = Scene;
            Scene = PreviousScene;
            PreviousScene = currentScene;
        }

        private void Clear()
        {
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
        }
    }
}
