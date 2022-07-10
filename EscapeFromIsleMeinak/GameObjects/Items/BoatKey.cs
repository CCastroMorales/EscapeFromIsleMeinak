using EscapeFromIsleMainak.Engine;

namespace EscapeFromIsleMainak.GameObjects
{
    public class BoatKey : Item
    {
        public BoatKey() 
        {
            Id = Id.ITEM_BOAT_KEY;
            Name = "Boat 46 Key";
            InventoryLabel = "Boat 46 Key";
            Description = "One of the keys hanging from the wall is marked 46.";
            InventoryDescription = "";
            Labels.AddRange(new string[] { "key" });
            LoseOnUse = true;
        }
    }
}
