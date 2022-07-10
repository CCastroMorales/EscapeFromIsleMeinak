using EscapeFromIsleMainak.Engine;
using EscapeFromIsleMainak.GameObjects;
using System;
using System.Collections.Generic;

namespace EscapeFromIsleMainak
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
