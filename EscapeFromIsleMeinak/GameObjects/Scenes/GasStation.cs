using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
{
    public class SceneGasStation : Scene
    {
        public override void OnLoad()
        {
            AddExit(Id.SCENE_VILLAGE_PROMENADE, Id.ENTITY_GAS_STATION_PERSON, new string[] { "promenade" });
            SpawnEntity<StationPerson>();
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_GAS_STATION;
        }
    }
}
