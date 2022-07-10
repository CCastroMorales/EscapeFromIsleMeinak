using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class Room : CheckObject
    {
        public Room()
        {
            Id = Id.CHECK_OBJECT_BUNGALOW_ROOM;
            Name = "Room";
            Labels.Add("room");
            Description = "The bungalow room is tiny. There is only room for two beds, a small cabinet with a television on top that barely fits. You notice the open BATHROOM door that's next to the entrance DOOR.";
        }
    }
}
