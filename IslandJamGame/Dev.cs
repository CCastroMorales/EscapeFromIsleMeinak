using IslandJamGame.Engine;
using IslandJamGame.GameObjects;
using System;
using System.Collections.Generic;

namespace IslandJamGame
{
    public class Dev
    {
        public static void SpawnJeepKey(List<Item> inventory)
        {
            Item item = new JeepKey();
            inventory.Add(item);
        }
    }
}
