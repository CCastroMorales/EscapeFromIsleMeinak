using EscapeFromIsleMeinak.GameObjects;
using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class SceneNeighbourBungalowBalcony : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_NEIGHBOUR_BUNGALOW_BALCONY;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_INBETWEEN_BUNGALOWS, new string[] { "down", "stairs" });
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_ROOM, Id.ENTITY_GAS_STATION_PERSON, new string[] { "room", "inside", "in" });
            SpawnEntity<ShadowyPerson>();
        }

        public override void OnRestore()
        {
            Entities.Clear();
            SpawnEntity<ShadowyPerson>();
        }
    }

    public class SceneNeighbourBungalowRoom : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_NEIGHBOUR_BUNGALOW_ROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_BALCONY, new string[] { "balcony", "outside", "out" });
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_BATHROOM, new string[] { "bathroom", "toilet", "bath" });
            SpawnItem<JeepKey>();
        }
    }

    public class SceneNeighbourBungalowBathroom : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_NEIGHBOUR_BUNGALOW_BATHROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_ROOM, new string[] { "room", "out" });
            RegisterCheckObject<Mirror>();
        }
    }
}
