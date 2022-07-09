using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class SceneBetweenBungalows : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_INBETWEEN_BUNGALOWS;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_BUNGALOW_BALCONY, "home");
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_BALCONY, "bungalow");
            AddExit(Id.SCENE_SPECIAL_VEHICLE_JEEP, "jeep");
        }
    }
}
