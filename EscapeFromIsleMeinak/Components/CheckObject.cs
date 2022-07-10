using System.Collections.Generic;

namespace EscapeFromIsleMeinak.Components
{
    public abstract class CheckObject
    {
        public Id Id { get; set; }
        public string Name { get; set; } = "";
        public List<string> Labels { get; set; } = new List<string>();
        public string Description { get; set; } = "";
        public List<Item> Items { get; set; } = new List<Item>();

        public Item SpawnItem<T>() where T : Item, new()
        {
            T t = new T();
            Item item = t;
            Items.Add(item);
            return item;
        }
    }
}
