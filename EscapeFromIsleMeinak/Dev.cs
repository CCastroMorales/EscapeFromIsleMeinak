using EscapeFromIsleMeinak.Components;
using EscapeFromIsleMeinak.GameObjects;
using System;
using System.Collections.Generic;

namespace EscapeFromIsleMeinak
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
