﻿using System.Collections.Generic;

namespace IslandJamGame.Engine
{
    public class Entity
    {
        public Id Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string TriggerDescription { get; set; } = "";
        public bool TriggerGameOver { get; set; } = false;
        public bool Dead { get; set; } = false;
        public bool ShowDescriptionWhenDead { get; set; } = true;
        public List<ItemType> KillBy { get; set; } = new List<ItemType>();
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
    }
}
