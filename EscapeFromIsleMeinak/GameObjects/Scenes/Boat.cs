using EscapeFromIsleMainak.Components;
using System;

namespace EscapeFromIsleMainak.GameObjects
{
    public class SceneBoat : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_HARBOR_JETTY, new string[] { "jetty", "harbor", "harbour", "pier", "out" });
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_SPECIAL_VEHICLE_BOAT;
        }
    }

    public class SceneBoatDriving : Scene
    {
        public override void OnLoad()
        {
            // There are no exits for this scene.
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_SPECIAL_VEHICLE_BOAT_DRIVING;
        }
    }
}
