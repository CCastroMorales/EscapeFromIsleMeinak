using EscapeFromIsleMeinak.GameObjects;
using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class SceneHarborEntrance : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_VILLAGE_BEACH, new string[] { "beach", "down", "right", "east" });
            AddExit(Id.SCENE_VILLAGE_BAR, new string[] { "bar", "left", "west" });
            AddExit(Id.SCENE_HARBOR_JETTY, new string[] { "jetty", "pier", "south" });
            AddExit(Id.SCENE_HARBOR_SHED, Id.ENTITY_SHED_PERSON, new string[] { "shed", "hut", "house", "building" });
            SpawnEntity<ShedPerson>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_HARBOR_ENTRANCE;
        }
    }

    public class SceneHarborShed : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_HARBOR_ENTRANCE, new string[] { "out", "harbor", "entrance", "pier", "jetty" });
            RegisterCheckObject<LockBox>().SpawnItem<BoatKey46>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_HARBOR_SHED;
        }
    }

    public class SceneHarborJetty : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_HARBOR_ENTRANCE, new string[] { "harbor", "entrance", "village", "north" });
            AddExit(Id.SCENE_SPECIAL_VEHICLE_BOAT, Id.ENTITY_JETTY_PERSON, new string[] { "boat", "46" });
            SpawnEntity<JettyPerson>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_HARBOR_JETTY;
        }
    }
}
