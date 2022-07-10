using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMainak.GameObjects
{
    public class SceneHarborEntrance : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_VILLAGE_BEACH, new string[] { "beach", "down", "right", "east" });
            AddExit(Id.SCENE_VILLAGE_BAR, new string[] { "bar", "left", "west" });
            AddExit(Id.SCENE_HARBOR_JETTY, new string[] { "jetty", "pier", "south" });
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
            AddExit(Id.SCENE_HARBOR_ENTRANCE, new string[] { "harbor", "entrance", "village", "north" });
            AddExit(Id.SCENE_SPECIAL_VEHICLE_BOAT, new string[] { "boat", "46" });
            SpawnItem<BoatKey>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_HARBOR_JETTY;
        }
    }
}
