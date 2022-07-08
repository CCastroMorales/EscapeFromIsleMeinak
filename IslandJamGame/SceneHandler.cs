using IslandJamGame.Engine;
using IslandJamGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IslandJamGame
{
    public class SceneHandler
    {
        public Scene Active { get; set; }
        public Scene Previous { get; set; }
        public List<Scene> Scenes { get; set; } = new List<Scene>();
        public Scene First { get => Scenes[0]; }

        public SceneHandler()
        {
            AddScene<BungalowRoom>();
            AddScene<BungalowBathroom>();
            AddScene<BungalowBalcony>();
            AddScene<BetweenBungalows>();
            AddScene<NeighbourBungalowBalcony>();
            AddScene<NeighbourBungalowRoom>();
            AddScene<NeighbourBungalowBathroom>();
        }

        public void AddScene<T>() where T : Scene, new()
        {
            Scene scene = new T();
            scene.Load();
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
    }
}
