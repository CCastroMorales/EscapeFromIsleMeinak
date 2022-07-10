using System.Collections.Generic;

namespace EscapeFromIsleMeinak.Components
{
    public struct Ctx
    {
        public Game Game { get; set; }
        /// <summary>
        /// The currently loaded scene.
        /// </summary>
        public Scene Scene { get; set; }
        public Inventory Inventory { get; set; }

        public Ctx(Game game, Scene scene, Inventory inventory)
        {
            Game = game;
            Scene = scene;
            Inventory = inventory;
        }
    }
}
