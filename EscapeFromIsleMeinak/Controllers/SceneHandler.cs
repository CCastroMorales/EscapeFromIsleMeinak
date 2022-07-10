using EscapeFromIsleMainak.Engine;
using EscapeFromIsleMainak.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace EscapeFromIsleMainak
{
    public class SceneHandler
    {
        public Scene Active { get; set; }
        public Scene Previous { get; set; }
        public List<Scene> Scenes { get; set; } = new List<Scene>();
        public Scene First { get => Scenes[0]; }
        public Scripting Parser { get; } = new Scripting();

        public SceneHandler()
        {
            // Ugh... This could've been handled by scripting or recursion.
            AddScene<SceneBungalowRoom>();
            AddScene<SceneBungalowBathroom>();
            AddScene<SceneBungalowBalcony>();
            AddScene<SceneBetweenBungalows>();
            AddScene<SceneNeighbourBungalowBalcony>();
            AddScene<SceneNeighbourBungalowRoom>();
            AddScene<SceneNeighbourBungalowBathroom>();
            AddScene<SceneJeep>();
            AddScene<SceneJeepDriving>();
            AddScene<SceneGasStation>();
            AddScene<SceneVillagePromenade>();
            AddScene<SceneVillageBeach>();
            AddScene<SceneHarborEntrance>();
            AddScene<SceneHarborJetty>();
            AddScene<SceneBoat>();
            AddScene<SceneBoatDriving>();
        }

        public void AddScene<T>() where T : Scene, new()
        {
            Scene scene = new T();
            scene.Load();
            LoadScript(scene);
            Scenes.Add(scene);
        }

        public Scene LoadFirstScene()
        {
            Active = First;
            return Active;
        }

        public Scene LoadScene(Id id)
        {
            foreach (Scene scene in Scenes)
                if (scene.Id == id)
                {
                    Previous = Active;
                    Active = scene;
                    return Active;
                }

            Active = null;
            return null;
        }

        /// <summary>
        /// Works as a save state. Used for game overs.
        /// </summary>
        public void RestorePreviousScene()
        {
            Active = Previous;
            Previous = null;
        }

        /// <summary>
        /// Switches between Active and Previous scenes.
        /// </summary>
        public void LoadPreviousScene()
        {
            Scene activeScene = Active;
            Active = Previous;
            Previous = activeScene;
        }

        public void LoadScript(Scene scene)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string path = $"EscapeFromIsleMeinak.res.{scene.Id}.script";

            bool resourceExits = false;
            foreach (string resourceName in assembly.GetManifestResourceNames())
                if (resourceName == path)
                {
                    resourceExits = true;
                    break;
                }

            if (!resourceExits)
            {
                string message = $"Resource {path} is not embedded.";
                Debug.WriteLine(message);
                throw new Exception(message);
            }

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                using (var reader = new StreamReader(stream))
                {
                    string scriptData = reader.ReadToEnd();
                    Parser.Scene = scene;
                    Parser.Parse(scriptData);
                }
            }
        }
    }
}
