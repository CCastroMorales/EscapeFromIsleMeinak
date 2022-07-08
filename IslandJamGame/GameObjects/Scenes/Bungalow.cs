using IslandJamGame.Engine;
using System;

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
            AddExit(Id.SCENE_INBETWEEN_BUNGALOWS, "down");
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

    public class BungalowRoom : Scene
    {
        public BungalowRoom()
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
