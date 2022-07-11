using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak
{
    public class Use : IParser
    {
        public bool Parse(Ctx ctx, InputBundle input)
        {
            if (input.Command != Commands.USE)
                return false;

            if (!input.HasArguments)
            {
                ctx.Game.OnPrint("Use what?");
                return false;
            }

            // Figure out what the target is.
            string target = ParseTarget(input.Arguments);

            if (UseItemOnTarget(ctx, input.FirstArgument, target))
                return false;
            //else if ()
            else if (UseKeyInVehicle(ctx, input.FirstArgument))
                return true;
            
            ctx.Game.OnPrint("You can't use it like that.");
            return false;
        }

        private bool UseKeyInVehicle(Ctx ctx, string itemName)
        {
            Item item = ctx.Inventory.FindItem(itemName);
            
            if (item == null)
                return false;

            if (item.Id == Id.ITEM_JEEP_KEY)
            {
                if (ctx.Scene.Id == Id.SCENE_SPECIAL_VEHICLE_JEEP)
                {
                    // Hack: transfer glove compartment.
                    var gloveCompartment = ctx.Scene.FindCheckObject(Id.CHECK_OBJECT_COMPARTMENT);
                    Id nextSceneId = Id.SCENE_SPECIAL_VEHICLE_JEEP_DRIVING;
                    Scene drivingScene = ctx.Game.Scenes.LoadScene(nextSceneId);
                    drivingScene.Objects.Add(gloveCompartment);

                    ctx.Game.Scenes.Previous = null;
                    ctx.Game.ItemUsed(item, true);
                    return true;
                }
                /*else
                    ctx.Game.PrintLine("Try using it in a jeep.");*/
                return false;
            }

            if (item.Id == Id.ITEM_BOAT_KEY_46)
            {
                if (ctx.Scene.Id == Id.SCENE_SPECIAL_VEHICLE_BOAT)
                {
                    LoadBoatDrivingScene(ctx, item);
                    return true;
                }
                /*else
                    ctx.Game.PrintLine("Try using it in a boat.");*/
                return false;
            }

            // Hack to make key 46 work despite key 86 being in inv.
            if (item.Id == Id.ITEM_BOAT_KEY_86)
            {
                if (ctx.Scene.Id == Id.SCENE_SPECIAL_VEHICLE_BOAT)
                {
                    // Search for key 46
                    item = ctx.Inventory.FindItem(Id.ITEM_BOAT_KEY_46);

                    if (item != null)
                    {
                        LoadBoatDrivingScene(ctx, item);
                        return true;
                    }
                    else
                        return false;
                } else
                    return false;
            }
            
            return false;
        }

        private static void LoadBoatDrivingScene(Ctx ctx, Item item)
        {
            ctx.Game.Scenes.LoadScene(Id.SCENE_SPECIAL_VEHICLE_BOAT_DRIVING);
            ctx.Game.Scenes.Previous = null;
            ctx.Game.ItemUsed(item, true);
        }

        private string ParseTarget(string[] arguments)
        {
            if (arguments.Length == 1)
                return null;
            if (arguments.Length == 2)
                return arguments[1];
            if (arguments.Length >= 3 && arguments[1] == "on")
                return arguments[2];
            return null;
        }

        private bool UseItemOnTarget(Ctx ctx, string itemName, string target)
        {
            Item item = ctx.Inventory.FindItem(itemName);
            
            if (target == null)
            {
                if (item != null)
                    if (item.Type == ItemType.WEAPON_MELEE || item.Type == ItemType.WEAPON_FIREARM)
                    {
                        ctx.Game.PrintLine("What do you want to use it on?");
                        return true;
                    }
                return false;
            }

            Entity entity = ctx.Scene.FindEntity(target);

            if (item == null || entity == null)
                return false;

            if (item.Type == ItemType.WEAPON_FIREARM && item.UsesRemaining == 0)
            {
                ctx.Game.PrintLine("It's out of ammunition!");
                return true;
            }
            else if (ctx.Game.KillEntityWith(entity, item.Type))
            {
                ctx.Game.ItemUsed(item, false);
                return true;
            }
            else if (entity.PacifyWith.Contains(item.Type))
            {
                ctx.Game.PrintLine(entity.PacifyDescription);
                entity.Pacify();

                ctx.Game.ItemUsed(item, false);
                return true;
            }

            return false;
        }

        private bool KillEntityWith(Ctx ctx, Entity entity, ItemType itemType)
        {
            if (entity.KillBy.Contains(itemType))
            {
                ctx.Game.PrintLine(entity.KilledDescription);
                Item drop = entity.Kill();
                ctx.Scene.Entities.Remove(entity);

                if (drop != null)
                {
                    ctx.Game.PrintLine(drop.Description);
                    ctx.Scene.Items.Add(drop);
                }

                return true;
            }
            return false;
        }
    }
}
