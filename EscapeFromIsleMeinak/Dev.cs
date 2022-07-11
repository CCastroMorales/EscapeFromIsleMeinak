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

        public static void SpawnKeys(Inventory inventory)
        {
            Item item = new BoatKey86();
            inventory.Add(item);
            
            item = new BoatKey46();
            inventory.Add(item);
        }

        public static void SpawnNote(Inventory inventory)
        {
            Item item = new Note();
            inventory.Add(item);
        }

        public static void SpawnPistol(Inventory inventory)
        {
            Item item = new Pistol();
            inventory.Add(item);
        }

        public static void SpawnBottle(Inventory inventory)
        {
            Item item = new Bottle();
            inventory.Add(item);
        }
    }
}
