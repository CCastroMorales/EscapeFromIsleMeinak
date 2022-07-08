using System.Collections.Generic;

namespace IslandJamGame.Engine
{
    public class Item
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Description { get; set; } = "";
        public string InventoryDescription { get; set; } = "";
        public List<ItemAction> Actions { get; set; } = new List<ItemAction>();
        public List<string> Labels { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }

        public bool HasAction(string hasAction)
        {
            foreach (ItemAction action in Actions)
                if (action.Action == hasAction)
                    return true;
            return false;
        }

        public ItemAction GetAction(string actionId)
        {
            foreach (ItemAction action in Actions)
                if (action.Action == actionId)
                    return action;
            return null;
        }
    }
}
