﻿using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class NeighbourBungalowBalcony : Scene
    {
        public NeighbourBungalowBalcony()
        {
            Id = Id.SCENE_NEIGHBOUR_BUNGALOW_BALCONY;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_INBETWEEN_BUNGALOWS, new string[] { "down", "stairs" });
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_ROOM, new string[] { "room", "inside", "in" });
        }
    }

    public class NeighbourBungalowRoom : Scene
    {
        public NeighbourBungalowRoom()
        {
            Id = Id.SCENE_NEIGHBOUR_BUNGALOW_ROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_BALCONY, new string[] { "balcony", "outside", "out" });
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_BATHROOM, new string[] { "bathroom", "toilet", "bath" });
            SpawnItem<JeepKey>();
        }
    }

    public class NeighbourBungalowBathroom : Scene
    {
        public NeighbourBungalowBathroom()
        {
            Id = Id.SCENE_NEIGHBOUR_BUNGALOW_BATHROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_NEIGHBOUR_BUNGALOW_ROOM, new string[] { "room", "out" });
        }
    }
}
