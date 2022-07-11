using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class LockBox : CheckObject
    {
        public LockBox()
        {
            Id = Id.CHECK_OBJECT_LOCK_BOX;
            Name = "Lock Box";
            Description = "You look inside the lock box. ITEM_DESCRIPTIONS";
            Labels.AddRange(new string[] { "lock", "box", "keybox", "key" });
        }
    }

    public class Compartment : CheckObject
    {
        public Compartment()
        {
            Id = Id.CHECK_OBJECT_COMPARTMENT;
            Name = "Compartment";
            Description = "You peek inside the compartment. Hm, there's not much in there. ITEM_DESCRIPTIONS";
            Labels.AddRange(new string[] { "compartment", "box" });
        }
    }

    public class Bed : CheckObject
    {
        public Bed()
        {
            Id = Id.CHECK_OBJECT_BED;
            Name = "Bed";
            Labels.Add("bed");
            Description = "You see a silhouette that resembles your friend on the bed. You touch your friend gently but realize that it was all an illusion. It's just the cover and pillows. Your friend is not here.";

        }
    }

    public class Mirror : CheckObject
    {
        public Mirror()
        {
            Id = Id.CHECK_OBJECT_MIRROR;
            Name = "Mirror";
            Description = "Is the message on the mirror written with lipstick? The full moon light allows you to barely read what it says: \"The meinak... it is real. i saw it floating head. it speak to me. If you read dis escape!\". You look out through the small window and see a strange orb moving slowly in the forest.";
            Labels.AddRange(new string[] { "mirror", "message" });
        }
    }

    public class Motor : CheckObject
    {
        public Motor()
        {
            Id = Id.CHECK_OBJECT_MOTOR;
            Name = "Motor";
            Description = "You open the hood and look at the motor and wonder why you're pretending to be a mechanic. You don't know what you're looking at and close the hood again.";
            Labels.AddRange(new string[] { "motor", "engine" });
        }
    }

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

    public class Body : CheckObject
    {
        public Body()
        {
            Id = Id.CHECK_OBJECT_BEACH_BODY;
            Name = "Body";
            Labels.AddRange(new string[] { "body", "person", "zombie", "zombi", "dead" });
            Description = "The person seems to have passed out. ITEM_DESCRIPTIONS";
        }
    }
}
