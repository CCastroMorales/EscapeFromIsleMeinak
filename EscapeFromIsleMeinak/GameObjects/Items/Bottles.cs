using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMainak.GameObjects
{
    public abstract class AbsBottle : Item
    {
        protected AbsBottle()
        {
            Type = ItemType.WEAPON_MELEE;
            Labels.Add("bottle");
            UsesRemaining = 3;
            LoseOnNoRemainingUses = true;
            LoseOnUse = false;
        }
    }

    public class Bottle : AbsBottle
    {
        public Bottle()
        {
            Id = Id.ITEM_BOTTLE;
            Name = "Bottle";
            InventoryLabel = "Bottle";
            Description = "There is a BOTTLE on one of the tables.";
            InventoryDescription = "Who knows who drank out of this bottle. Better not drink out of it.";
        }
    }

    public class BrokenBottle : AbsBottle
    {
        public BrokenBottle()
        {
            Id = Id.ITEM_BROKEN_BOTTLE;
            Name = "Broken bottle";
            UsesRemaining = 2;
            InventoryLabel = "Brk. Bottle";
            Description = "There is a broken BOTTLE on the sink.";
            InventoryDescription = "You don't drink so this bottle was left and broken by your friend.";
        }
    }
}
