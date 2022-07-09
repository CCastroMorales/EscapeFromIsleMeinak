using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace IslandJamGame
{
    public static class TitleScreen
    {
        public static void Display()
        {
            Console.CursorVisible = false;

            string titleSuper = Strings.SUPERSCRIPT_TITLE;
            string[] titleAscii = LoadAscii();

            int x = (Console.BufferWidth / 2) - (titleAscii[0].Length / 2);
            int y = Console.WindowHeight / 4;

            Console.SetCursorPosition(x, y - 1);
            Console.WriteLine(titleSuper);

            foreach (string line in titleAscii)
            {
                Console.SetCursorPosition(x, y);
                Console.WriteLine(line);
                y++;
            }

            string text = Strings.PROMPT_ENTER_TO_PLAY;
            x = (Console.BufferWidth / 2) - (text.Length / 2);

            Thread.Sleep(Timing.GameOverPressPromptDelay);
            Console.SetCursorPosition(x, y + 3);
            Console.WriteLine(text);

            var readkey = Console.ReadKey(true);
            while (readkey.Key != ConsoleKey.Enter)
            {
                readkey = Console.ReadKey(true);
            }
        }

        public static string[] LoadAscii()
        {
            string[] ascii;

            var assembly = Assembly.GetExecutingAssembly();
            string path = $"IslandJamGame.res.TITLE_ASCII.txt";

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    string data = reader.ReadToEnd();
                    ascii = data.Split('\n');
                }
            }

            return ascii;
        }
    }
}
