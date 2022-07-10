﻿using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMainak.GameObjects
{
    public class SceneHarborEntrance : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_VILLAGE_BEACH, new string[] { "beach" });
            AddExit(Id.SCENE_HARBOR_JETTY, new string[] { "jetty" });
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_HARBOR_ENTRANCE;
        }
    }

    public class SceneHarborJetty : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_HARBOR_ENTRANCE, new string[] { "harbor", "entrance", "village" });
            AddExit(Id.SCENE_SPECIAL_VEHICLE_BOAT, new string[] { "boat", "46" });
            SpawnItem<BoatKey>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_HARBOR_JETTY;
        }
    }
}
