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
            Description = "You spot a key on the table. They resemble car keys.";
            InventoryDescription = "";
            Labels.AddRange(new string[] { "key", "keys" });
            LoseOnUse = true;
        }        
    }
}
