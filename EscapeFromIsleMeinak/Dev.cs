using EscapeFromIsleMeinak.Components;
using EscapeFromIsleMeinak.GameObjects;
using System;
using System.Collections.Generic;

namespace EscapeFromIsleMeinak
{
    public class Dev
    {
        public static void SpawnJeepKey(Inventory inventory)
        {
            Item item = new JeepKey();
            inventory.Add(item);
        }
    }
}
