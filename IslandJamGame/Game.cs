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
        public Scene PreviousScene { get; set; }
        public bool Running { get; set; } = true;
        public int DefaultSleepMillis { get; set; } = 100;
        public int DefaultTitleSleepMillis { get; set; } = 1000;
        public Scene Scene { get; set; }
        public bool FastForward { get; set; } = false;
        public ConsoleColor DefaultConsoleColor { get; } = Console.ForegroundColor;

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

            Console.WriteLine($"{Console.WindowWidth}");
            Console.WriteLine($"{Console.WindowHeight}");

            //Test();
            /*Console.ReadLine();

            Console.WriteLine("hello");

            for (int i = 5; i > 0; i--)
            {
                Thread.Sleep(500);
                Console.SetCursorPosition(i, 2);
                Console.WriteLine("      ");
            }*/

            while (Running)
            {
                Clear();
                PlayScene(Scene);
                PresentOptions(Scene);
                ParseInput(Scene);

                /*if (Scene.NextScene != "")
                    LoadNextScene(Scene.NextScene);*/

                /*Console.WriteLine("Hello world!");

                ConsoleColor foreColor = Console.ForegroundColor;
                ConsoleColor backColor = Console.BackgroundColor;
                Console.WriteLine("Clearing the screen!");
                ConsoleColor newForeColor = ConsoleColor.Blue;
                ConsoleColor newBackColor = ConsoleColor.Yellow;

                Console.WriteLine("Hello world!");
                Console.ReadLine();
                Clear();*/

                //Console.ReadLine();

                /*if (Delay >= DelayCounter)
                    DelayCounter = 0;*/
            }

        }

        private void Test()
        {
            // Get an array with the values of ConsoleColor enumeration members.
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            // Save the current background and foreground colors.
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;

            // Display all foreground colors except the one that matches the background.
            Console.WriteLine("All the foreground colors except {0}, the background color:",
                              currentBackground);
            foreach (var color in colors)
            {
                if (color == currentBackground) continue;

                Console.ForegroundColor = color;
                Console.WriteLine("   The foreground color is {0}.", color);
            }
            Console.WriteLine();
            // Restore the foreground color.
            Console.ForegroundColor = currentForeground;

            // Display each background color except the one that matches the current foreground color.
            Console.WriteLine("All the background colors except {0}, the foreground color:",
                              currentForeground);
            foreach (var color in colors)
            {
                if (color == currentForeground) continue;

                Console.BackgroundColor = color;
                Console.WriteLine("   The background color is {0}.", color);
            }

            // Restore the original console colors.
            Console.ResetColor();
            Console.WriteLine("\nOriginal colors restored...");
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
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(scene.Title);
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
            PrintText(scene.text);
        }

        private void PrintText(List<string> lines)
        {
            PrintText(lines.ToArray());
        }

        private void PrintText(string[] lines)
        {
            foreach (string text in lines)
            {
                string[] words = text.Split(' ');

                foreach (string word in words)
                {
                    if (IsCapitalized(word))
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = DefaultConsoleColor;

                    foreach (Char c in word)
                    {
                        Console.Write(c);

                        if (c == '.' || c == ',' || c == ';' || c == ':' || c == '?' || c == '"')
                        {
                            if (!FastForward)
                                Thread.Sleep(1000);
                        }
                        else
                            if (!FastForward)
                            Thread.Sleep(25);
                    }

                    Console.Write(' ');
                    //Console.Write("\n");
                    //Console.WriteLine("");
                }

                //Console.WriteLine(text);
                Console.Write("\n");
                Console.WriteLine("");
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
