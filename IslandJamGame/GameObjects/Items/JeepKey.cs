using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class JeepKey : Item
    {
        public JeepKey()
        {
            Id = Id.ITEM_JEEP_KEY;
            Name = "Jeep Key";
            InventoryLabel = "Jeep Key";
            Description = "You spot keys on the table. The resemble car keys.";
            InventoryDescription = "The note is from your friend; you recognize their sloppy handwriting.";
            Labels.AddRange(new string[] { "key", "keys" });
        }        
    }
}
