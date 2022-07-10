using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
{
    public abstract class Human : Entity
    {
        protected Human()
        {
            KillBy.Add(ItemType.WEAPON_MELEE);
            KillBy.Add(ItemType.WEAPON_FIREARM);
            Labels.AddRange(new string[] { "person", "zombie", "zombi", "figure", "it", "them" });

            Passive = false;
            PacifyWith.Add(ItemType.MEAT);
            PacifyDescription = "You throw the meat toward the person and it looks angrily at you. It smells the meat and starts devouring it. You can probably walk by while it is distracted.";
            ShowDescriptionWhenPassive = false;

            MaxKillAttempts = 1;
            KillAttempt = 0;
            KillFailDescriptions.Add("The person screams at you and tries to punch you but you manage to dodge. You back off as you realize that you might not be so lucky next time.");
            KillFailDescriptions.Add("You realize your mistake as the person turns toward you and launches forward to grab you. It succeeds. Is the person biting you?! You feel immense pain and you start to eventually feel dizzy. You realize that you have a large wound on your shoulder. It's over.");
        }
    }

    public class ShadowyPerson : Human
    {
        public ShadowyPerson()
        {
            Id = Id.ENTITY_GAS_STATION_PERSON;
            Name = "Person";
            Description = "You can see the shadowy figure hunching over the balcony railing.";
            TriggerDescription = "The person turns around and launches toward you. You try to flee inside but you slip and fall. The person grabs you from behind and you can't get free. Eventually you start to feel dizzy and realize that you have a large wound on your shoulder. It's over.";

            KilledDescription = "The person is not moving anymore. It must have been the person who lived in this bungalow. You have no memory of this person when arriving yesterday. You leave the person hanging over the railing.";
            ShowDescriptionWhenDead = false;
            TriggerGameOver = true;
        }
    }

    public class StationPerson : Human
    {
        public StationPerson()
        {
            Id = Id.ENTITY_GAS_STATION_PERSON;
            Name = "Person";
            Description = "There is a hunched down person near the road that leads to the village promenade.";
            TriggerDescription = "The person turns around and launches toward you. You can't escape. The person is somehow faster than you. The person eventually catches up to from behind and you can't get free. You realize that you have been bit somewhere and you pass out. It's over.";

            KilledDescription = "The person is not moving anymore. Was this your friend? No, the person looks nothing like them...";
            ShowDescriptionWhenDead = false;
            TriggerGameOver = true;
        }
    }
}
