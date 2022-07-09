using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class SceneVillagePromenade : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_GAS_STATION, new string[] { "gas", "station" });
            AddExit(Id.SCENE_VILLAGE_BEACH, new string[] { "beach" });
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_VILLAGE_PROMENADE;
        }
    }

    public class SceneVillageBeach : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_VILLAGE_PROMENADE, "promenade");
            AddExit(Id.SCENE_HARBOR_ENTRANCE, "harbor");
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_VILLAGE_BEACH;
        }
    }
}
