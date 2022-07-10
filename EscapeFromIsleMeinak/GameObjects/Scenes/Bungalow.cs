using EscapeFromIsleMainak.Components;
using EscapeFromIsleMeinak.GameObjects;
using System;

namespace EscapeFromIsleMainak.GameObjects
{
    public class SceneBungalowBalcony : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_BUNGALOW_BALCONY;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_BUNGALOW_ROOM, new string[] { "bedroom", "room", "in", "inside" });
            AddExit(Id.SCENE_INBETWEEN_BUNGALOWS, new string[] { "down", "stairs", "patio" });
        }
    }

    public class SceneBungalowBathroom : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_BUNGALOW_BATHROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_BUNGALOW_ROOM, new string[] { "room", "bedroom", "out" });
            SpawnItem<Note>();
            SpawnItem<BrokenBottle>();
        }

        
    }

    public class SceneBungalowRoom : Scene
    {
        public override Id OnRegisterId()
        {
            return Id.SCENE_BUNGALOW_ROOM;
        }

        public override void OnLoad()
        {
            AddExit(Id.SCENE_BUNGALOW_BATHROOM, new string[] { "bathroom", "toilet", "bath" });
            AddExit(Id.SCENE_BUNGALOW_BALCONY, new string[] { "outside", "out", "balcony" } );
            RegisterCheckObject<Room>();
            RegisterCheckObject<Bed>();
            SpawnEntity<Rat>();
        }
    }
}
