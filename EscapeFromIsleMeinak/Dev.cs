using MeinakEsc.Components;
using MeinakEsc.GameObjects;
using System;
using System.Collections.Generic;

namespace MeinakEsc
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
