using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace EscapeFromIsleMeinak
{
    public static class Display
    {
        public static void TitleScreen(bool debug, bool restarted, string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;

            string titleSuper = restarted ? Strings.SUPERSCRIPT_TITLE_ALT : Strings.SUPERSCRIPT_TITLE;
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

            Thread.Sleep(Timing.EndScreenPressPromptDelay);
            Console.SetCursorPosition(x, y + 3);
            Console.WriteLine(text);

            if (debug)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;

                string argline = "";
                foreach (string arg in args)
                    argline += $"{arg} ";

                x = (Console.BufferWidth / 2) - (argline.Length / 2);
                Console.SetCursorPosition(x, Console.CursorTop);
                Console.WriteLine(argline);
            }

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
            string path = $"EscapeFromIsleMeinak.res.TITLE_ASCII.txt";

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

        public static void EndScreen()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;

            string theEnd = Strings.THE_END.Replace(' ', '═');

            int x = (Console.BufferWidth / 2) - (theEnd.Length / 2);
            int y = (Console.WindowHeight / 3);

            Console.SetCursorPosition(0, y);

            for (int i = 0; i < x; i++)
            {
                Console.Write('═');
                Thread.Sleep(Timing.GameOverAnimationDuration);
            }

            for (int i = 0; i < theEnd.Length; i++)
            {
                Console.Write(theEnd[i]);
                Thread.Sleep(Timing.GameOverAnimationDuration);
            }

            for (int i = x + theEnd.Length; i < Console.BufferWidth; i++)
            {
                Console.Write('═');
                Thread.Sleep(Timing.GameOverAnimationDuration);
            }


            Console.ForegroundColor = ConsoleColor.DarkGray;

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

            Console.CursorVisible = true;
        }
    }
}
