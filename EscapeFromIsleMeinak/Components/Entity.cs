using System.Collections.Generic;

namespace EscapeFromIsleMeinak.Components
{
    public class Entity
    {
        public Id Id { get; set; }
        public string Name { get; set; } = "";
        public List<string> Labels { get; set; } = new List<string>();
        public string Description { get; set; } = "";
        public string TriggerDescription { get; set; } = "";
        public string KilledDescription { get; set; } = "";
        public string PassifyDescription { get; set; } = "";
        public bool TriggerGameOver { get; set; } = false;
        public bool Dead { get; set; } = false;
        public bool Passive { get; set; } = false;
        public bool ShowDescriptionWhenDead { get; set; } = true;
        public bool ShowDescriptionWhenPassive { get; set; } = false; 
        public List<ItemType> KillBy { get; set; } = new List<ItemType>();
        public List<ItemType> PassifyWith { get; set; } = new List<ItemType>();
        public int MaxKillAttempts { get; set; } = 0;
        public int KillAttempt { get; set; } = 0;
        public List<string> KillFailDescriptions { get; set; } = new List<string>();
        public Item DropItem { get; set; } = null;
        public bool HasDropItem { get => DropItem != null; }

        public override string ToString()
        {
            return Id.ToString();
        }

        public Item Kill()
        {
            Dead = true;
            return DropItem;
        }

        public void Passify()
        {
            Passive = true;
        }
    }
}
