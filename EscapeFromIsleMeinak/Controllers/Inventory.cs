using EscapeFromIsleMeinak.Components;
using System.Collections.Generic;

namespace EscapeFromIsleMeinak
{
    public class Inventory
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public int Count { get => Items.Count; }

        public Item FindItem(string searchLabel)
        {
            foreach (Item item in Items)
                if (item.Labels.Contains(searchLabel))
                    return item;
            return null;
        }

        public Item FindItem(Id id)
        {
            foreach (Item item in Items)
                if (item.Id == id)
                    return item;
            return null;
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }

        public void Remove(Item item)
        {
            Items.Remove(item);
        }


    }
}
