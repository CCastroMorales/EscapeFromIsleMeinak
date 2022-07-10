using EscapeFromIsleMainak.Engine;

namespace EscapeFromIsleMainak
{
    public class Window
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            ParseCommandLineArguments(args, game);
            game.Run(args);
        }

        static void ParseCommandLineArguments(string[] args, Game game)
        {
            int i = 0;
            if (args.Length > 0)
                foreach (string arg in args)
                {
                    if (arg == "+ff")
                        game.FastForward = true;
                    if (arg == "+debug")
                        game.Debug = true;
                    if (arg == "+jeepkey")
                        Dev.SpawnJeepKey(game.Inventory);
                    if (arg == "+scene")
                    {
                        if (i <= args.Length - 2)
                        {
                            string sceneId = args[i + 1];
                            Id id;
                            Id.TryParse(sceneId, out id);

                            Scene scene = game.Scenes.LoadScene(id);
                            game.Scenes.Scenes.Remove(scene);
                            game.Scenes.Scenes.Insert(0, scene);
                        }
                    }
                    i++;
                }
        }
    }
}
