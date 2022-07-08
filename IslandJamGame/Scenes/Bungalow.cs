using IslandJamGame.Engine;
using System;

namespace IslandJamGame.Scenes
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
        }
    }
}
