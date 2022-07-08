using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class ShadowyZombie : Entity
    {
        public ShadowyZombie()
        {
            Id = Id.ENTITY_BUNGALOW_SHADOWY_ZOMBIE;
            Name = "Person";
            Description = "You can see the shadowy figure hunching over the balcony railing.";
            KillBy.Add(ItemType.MELEE);
            ShowDescriptionWhenDead = false;
            DropItem = new PassedOutRat();
            TriggerDescription = "The person turns around and launches toward you. You try to flee inside but you slip and fall. The person grabs you from behind and you can't get free. Eventually you start to feel dizzy and realize that you have a large wound on your shoulder. It's over.";
            TriggerGameOver = true;
        }
    }
}
