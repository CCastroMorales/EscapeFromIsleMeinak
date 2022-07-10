using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMainak.GameObjects
{
    public class Bottle : Item
    {
        public Bottle()
        {
            Id = Id.ITEM_BOTTLE;
            Name = "Bottle";
            InventoryLabel = "Bottle";
            Type = ItemType.WEAPON_MELEE;
            Description = "There is a BOTTLE on one of the tables.";
            InventoryDescription = "Who knows who drank out of this bottle. Better not drink out of it.";
            // TODO Add actions
            Labels.Add("bottle");
        }
    }
}
