using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class JeepKey : Item
    {
        public JeepKey()
        {
            Id = Id.ITEM_JEEP_KEY;
            Name = "Jeep Key";
            InventoryLabel = "Jeep Key";
            Description = "You spot a KEY on the table. They resemble car keys.";
            InventoryDescription = "The key to a jeep; probably the one parked outside the bungalows.";
            Labels.AddRange(new string[] { "key", "keys", "jeepkey" });
            LoseOnUse = true;
        }
    }

    public class BoatKey46 : Item
    {
        public BoatKey46() 
        {
            Id = Id.ITEM_BOAT_KEY_46;
            Name = "Key 46";
            InventoryLabel = "Key 46";
            Description = "One of the KEYS hanging from the wall is marked 46.";
            InventoryDescription = "This must be the key that will start the motor on boat 46.";
            Labels.AddRange(new string[] { "key", "key46", "key 46" });
            LoseOnUse = true;
        }
    }

    public class BoatKey86 : Item
    {
        public BoatKey86()
        {
            Id = Id.ITEM_BOAT_KEY_86;
            Name = "Key 86";
            InventoryLabel = "Key 86";
            Description = "There is a KEY dangling from the persons belt.";
            InventoryDescription = "This must be the key that will start the motor on boat 86.";
            Labels.AddRange(new string[] { "key", "key86", "key 86" });
            LoseOnUse = true;
        }
    }
}
