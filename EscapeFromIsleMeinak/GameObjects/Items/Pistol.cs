using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMainak.GameObjects
{
    public class Pistol : Item
    {
        public Pistol()
        {
            Id = Id.ITEM_PISTOL;
            Name = "Pistol";
            UsesRemaining = 3;
            InventoryLabel = "Pistol";
            Type = ItemType.WEAPON_FIREARM;
            Description = "There is a PISTOL in the compartment.";
            InventoryDescription = "Using a gun is easy, just pull the trigger... right?";
            Labels.AddRange(new string[] { "pistol", "gun" });
            LoseOnNoRemainingUses = false;
        }
    }
}
