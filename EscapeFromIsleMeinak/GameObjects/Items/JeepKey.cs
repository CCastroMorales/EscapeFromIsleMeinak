using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
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
            Labels.AddRange(new string[] { "key", "keys" });
            LoseOnUse = true;
        }        
    }
}
