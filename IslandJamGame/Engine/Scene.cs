using IslandJamGame.Scenes;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IslandJamGame.Engine
{
    public abstract class Scene
    {
        public Id Id { get; set; }
        public string Title { get; set; } = "";
        public List<string> Text { get; } = new List<string>();
        public List<Item> Items { get; } = new List<Item>();
        public List<Entity> Entities { get; } = new List<Entity>();
        public List<Exit> Exits { get; } = new List<Exit>();

        public void LoadTextFromResource(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string path = $"IslandJamGame.res.{resource}";

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    string[] lines = json.Split('\n');
                    Text.AddRange(lines);
                }
            }
        }

        public void Load()
        {
            string id = Id.ToString();
            LoadTextFromResource($"{id}.txt");
            OnLoad();
        }

        public abstract void OnLoad();

        protected void AddExit(Id id, string command)
        {
            Exit exit = new Exit();
            exit.Destination = id;
            exit.Command = command;
            Exits.Add(exit);
        }

        public Exit FindExit(string command)
        {
            foreach (Exit exit in Exits)
                if (exit.Command == command)
                    return exit;
            return null;
        }
    }
}
