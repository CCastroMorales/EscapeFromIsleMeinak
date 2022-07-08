namespace IslandJamGame
{
    public class Window
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            ParseCommandLineArguments(args, game);
            game.Run();
        }

        static void ParseCommandLineArguments(string[] args, Game game)
        {
            if (args.Length > 0)
                foreach (string arg in args)
                {
                    if (arg == "+ff")
                        game.FastForward = true;
                    if (arg == "+debug")
                        game.Debug = true;
                }
        }
    }
}
