using MeinakEsc;
using MeinakEsc.Components;
using System;

namespace EscapeFromIsleMeinak
{
    public class Drop : IParser
    {
        public bool Parse(Ctx ctx, InputBundle input)
        {
            if (input.Command != Commands.DROP)
                return false;

            if (!input.HasArguments)
            {
                ctx.Game.OnPrint("Drop what?");
            }

            if (DropItem(ctx, input.FirstArgument))
                return false;
            return false;
        }

        private bool DropItem(Ctx ctx, string itemName)
        {
            Item item = ctx.Inventory.FindItem(itemName);

            if (item != null)
            {
                ctx.Inventory.Remove(item);
                ctx.Scene.DroppedItems.Add(item);
                ctx.Game.UpdateInventoryScreen();
                ctx.Game.PrintLine($"You dropped {item.Name.ToLower()}.");
                return true;
            }

            return false;
        }
    }
}
