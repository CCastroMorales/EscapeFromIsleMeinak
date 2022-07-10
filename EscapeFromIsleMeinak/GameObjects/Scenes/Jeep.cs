using EscapeFromIsleMeinak.GameObjects;
using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
{
    public class SceneJeep : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_SPECIAL_VEHICLE_JEEP;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_INBETWEEN_BUNGALOWS, new string[] { "patio", "between", "stairs", "inbetween", "out" });
            RegisterCheckObject<Compartment>().SpawnItem<Pistol>();
        }

    }

    public class SceneJeepDriving : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_GAS_STATION, new string[] { "gas", "station", "gas station", "distance" });
            RegisterCheckObject<Motor>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_SPECIAL_VEHICLE_JEEP_DRIVING;
        }
    }
}
