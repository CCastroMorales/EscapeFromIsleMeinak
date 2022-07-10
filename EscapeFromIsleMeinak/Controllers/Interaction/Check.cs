using EscapeFromIsleMainak;
using EscapeFromIsleMainak.Components;
using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak
{
    public class Check
    {
        /// <summary>
        /// Parses CHECK commands and prints the descriptions of objects, items and entities.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="input"></param>
        /// <returns>true to end reading user input.</returns>
        public bool Parse(Ctx ctx, InputBundle input)
        {
            if (input.Command != Commands.CHECK)
                return false;

            if (!input.HasArguments)
            {
                ctx.Game.OnPrint("Check what");
                return false;
            }

            if (CheckObject(ctx, input.FirstArgument))
                return false;
            else if (CheckSceneItem(ctx, input.FirstArgument))
                return false;
            else if (CheckSceneEntity(ctx, input.FirstArgument))
                return false;
            else
                ctx.Game.OnPrint("Huh?");

            return false;
        }

        private bool CheckObject(Ctx ctx, string objectName)
        {
            CheckObject @object = ctx.Scene.FindCheckObject(objectName.ToLower());

            if (@object != null)
            {
                ctx.Game.PrintLine(@object.Description);
                return true;
            }

            return false;
        }

        private bool CheckSceneItem(Ctx ctx, string objectName)
        {
            Item item = ctx.Scene.FindItem(objectName.ToLower());

            if (item != null)
            {
                ctx.Game.PrintLine(item.Description);
                return true;
            }

            return false;
        }

        private bool CheckSceneEntity(Ctx ctx, string entityName)
        {
            Entity entity = ctx.Scene.FindEntity(entityName.ToLower());

            if (entity != null)
            {
                ctx.Game.PrintLine(entity.Description);
                return true;
            }

            return false;
        }
    }
}
