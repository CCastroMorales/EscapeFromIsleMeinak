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
            AddExit(Id.SCENE_BUNGALOW_ROOM, new string[] { "bedroom", "room", "in", "inside" });
            AddExit(Id.SCENE_INBETWEEN_BUNGALOWS, new string[] { "down", "stairs", "patio" });
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
            AddExit(Id.SCENE_BUNGALOW_ROOM, new string[] { "room", "bedroom" });
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
            AddExit(Id.SCENE_BUNGALOW_BATHROOM, new string[] { "bathroom", "toilet", "bath" });
            AddExit(Id.SCENE_BUNGALOW_BALCONY, new string[] { "outside", "out", "balcony" } );
            SpawnEntity<Rat>();
        }
    }
}
