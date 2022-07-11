using EscapeFromIsleMeinak.GameObjects;
using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak.GameObjects
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
            AddExit(Id.SCENE_VILLAGE_PROMENADE, new string[] { "promenade", "road", "up " });
            AddExit(Id.SCENE_VILLAGE_BAR, new string[] { "bar" });
            AddExit(Id.SCENE_HARBOR_ENTRANCE, new string[] { "harbor", "entrance", "pier" });
            RegisterCheckObject<Body>().SpawnItem<BoatKey86>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_VILLAGE_BEACH;
        }
    }

    public class SceneVillageBar : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_VILLAGE_BEACH, "beach");
            AddExit(Id.SCENE_HARBOR_ENTRANCE, new string[] { "harbor", "entrance", "pier" });
            SpawnItem<Bottle>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_VILLAGE_BAR;
        }
    }
}
