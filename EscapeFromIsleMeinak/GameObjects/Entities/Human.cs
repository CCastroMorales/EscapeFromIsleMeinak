using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMainak.GameObjects
{
    public abstract class Human : Entity
    {
        // Do nothing
    }

    public class ShadowyPerson : Human
    {
        public ShadowyPerson()
        {
            Id = Id.ENTITY_BUNGALOW_SHADOWY_ZOMBIE;
            Name = "Person";
            Labels.AddRange(new string[] { "person", "zombie", "figure", "it", "them" });
            Description = "You can see the shadowy figure hunching over the balcony railing.";
            TriggerDescription = "The person turns around and launches toward you. You try to flee inside but you slip and fall. The person grabs you from behind and you can't get free. Eventually you start to feel dizzy and realize that you have a large wound on your shoulder. It's over.";

            KillBy.Add(ItemType.WEAPON_MELEE);
            KilledDescription = "You hit the person on the head. The person is not moving anymore. It must have been the person who lived in this bungalow. You have no memory of this person when arriving yesterday. You leave the person hanging over the railing.";
            ShowDescriptionWhenDead = false;
            TriggerGameOver = true;

            Passive = false;
            PassifyWith.Add(ItemType.MEAT);
            PassifyDescription = "You throw the meat toward the person and it looks angrily at you. It smells the meat and starts devouring it. You can probably walk by while it is distracted.";
            ShowDescriptionWhenPassive = false;

            MaxKillAttempts = 1;
            KillAttempt = 0;
            KillFailDescriptions.Add("The person screams at you and tries to punch you but you manage to dodge. You back off as you realize that you might not be so lucky next time.");
            KillFailDescriptions.Add("You realize your mistake as the person turns around and launches toward you and grabs you. Is the person biting you?! You feel immense pain and you start to eventually feel dizzy. You realize that you have a large wound on your shoulder. It's over.");
        }
    }
}
