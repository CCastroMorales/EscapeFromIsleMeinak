using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
{
    public abstract class Human : Entity
    {
        protected Human()
        {
            Name = "Person";

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
            Description = "There is a hunched down person near the road that leads to the village promenade.";
            TriggerDescription = "The person turns around and launches toward you. You can't escape. The person is somehow faster than you. The person eventually catches up to from behind and you can't get free. You realize that you have been bit somewhere and you pass out. It's over.";

            KilledDescription = "The person is not moving anymore. Was this your friend? No, the person looks nothing like them...";
            ShowDescriptionWhenDead = false;
            TriggerGameOver = true;
        }
    }

    public class JettyPerson : Human
    {
        public JettyPerson()
        {
            Id = Id.ENTITY_JETTY_PERSON;
            Description = "There is however a person next to the boat as well. The person... looks awfully similar to your... friend?! It can't be. There's something different about the person but the clothes are similar to your friend's style... and the face...";
            TriggerDescription = "The person looks directly at you and launches toward you. You don't know what to do; this is the enemy but it looks like your friend! You try to run away but you are overrun as other people start noticing you and flock around you. You know what is going to happen and you close your eyes. It's over.";

            KilledDescription = "The person... is not moving anymore. Was this your friend? What happe... How did they become like this and what is happening on this island?! You need to escape and quickly before you end up just like...... them.";
            ShowDescriptionWhenDead = false;
            TriggerGameOver = true;

            PacifyWith.Add(ItemType.NOTE);
            PacifyDescription = "In a desperate attempt you try to hand over the note to the person. The person looks at you and you immediately regret your decision.... The person doesn't attack you however. They stare at the note for a while. The person pushes away your hand, makes some noises and starts moving slowly toward the other people who are mesmerized by the boats and the decorations.";
        }
    }

    public class ShedPerson : Human
    {
        public ShedPerson()
        {
            Id = Id.ENTITY_SHED_PERSON;
            Description = "You find another person by the SHED. The person is standing close to the shed entrance.";
            TriggerDescription = "The person's head turns to you and it looks straight into your eyes. You see darkness. Before you know it you're trapped and the person pushes you down. You close your eyes because you know it is over...";

            KilledDescription = "The person is not moving anymore. You don't know how much longer you can hold on. Your sanity is dwindling away...";
            ShowDescriptionWhenDead = false;
            TriggerGameOver = true;
        }
    }
}
