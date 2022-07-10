using System.Collections.Generic;

namespace EscapeFromIsleMainak.Engine
{
    public class Item
    {
        public Id Id { get; set; }
        public string Name { get; set; } = "";
        public string LowerCaseName { get => Name.ToLower(); }
        public string InventoryLabel { get; set; } = "";
        public ItemType Type { get; set; }
        public string Description { get; set; } = "";
        public string InventoryDescription { get; set; } = "";
        public List<ItemAction> Actions { get; set; } = new List<ItemAction>();
        public List<string> Labels { get; set; } = new List<string>();
        public bool LoseOnUse { get; set; } = false;

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }

        public ItemAction GetAction(Id actionId)
        {
            foreach (ItemAction action in Actions)
                if (action.Id == actionId)
                    return action;
            return null;
        }
    }
}
