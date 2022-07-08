﻿using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class BrokenBottle : Item
    {
        public BrokenBottle()
        {
            Id = Id.ITEM_BROKEN_BOTTLE;
            Name = "Broken bottle";
            InventoryLabel = "Brk. Bottle";
            Type = ItemType.MELEE;
            Description = "There is a broken BOTTLE on the sink.";
            InventoryDescription = "You don't drink so this bottle was left and broken by your friend.";
            // TODO Add actions
            Labels.Add("bottle");
        }
    }
}
