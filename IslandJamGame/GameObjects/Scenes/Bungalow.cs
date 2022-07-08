using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class BungalowBalcony : Scene
    {
        public BungalowBalcony()
        {
            Id = Id.SCENE_BUNGALOW_BALCONY;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_BUNGALOW_ROOM, "inside");
            // Id.SCENE_INBETWEEN
        }
    }

    public class BungalowBathroom : Scene
    {
        public BungalowBathroom()
        {
            Id = Id.SCENE_BUNGALOW_BATHROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_BUNGALOW_ROOM, "room");
            SpawnItem<Note>();
            SpawnItem<BrokenBottle>();
        }
    }

    public class Bungalow : Scene
    {
        public Bungalow()
        {
            Id = Id.SCENE_BUNGALOW_ROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_BUNGALOW_BATHROOM, "bathroom");
            AddExit(Id.SCENE_BUNGALOW_BALCONY, "outside");
            SpawnEntity<Rat>();
        }
    }
}
