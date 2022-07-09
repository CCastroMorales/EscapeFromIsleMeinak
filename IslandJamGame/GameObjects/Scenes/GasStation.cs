using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class SceneGasStation : Scene
    {
        public override void OnLoad()
        {
            AllowGoBack = false;
            AddExit(Id.SCENE_VILLAGE_PROMENADE, "promenade");
        }

        public override Id OnRegisterId()
        {
            return Id.SCENE_GAS_STATION;
        }
    }
}
