using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class PassedOutRat : Item
    {
        public PassedOutRat()
        {
            Id = Id.ITEM_PASSED_OUT_RAT;
            Name = "Rat";
            InventoryLabel = "Psd.out. Rat";
            Type = ItemType.MEAT;
            Description = "There is a passed out RAT on the floor.";
            InventoryDescription = "You're carrying around a passed out rat. The stressful situation is clearly affecting your sanity.";
            Labels.Add("rat");
            LoseOnUse = true;
        }
    }
}
