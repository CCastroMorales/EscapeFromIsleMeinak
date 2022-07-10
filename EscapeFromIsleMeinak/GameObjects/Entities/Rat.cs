using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class Rat : Entity
    {
        public Rat()
        {
            Id = Id.ENTITY_BUNGALOW_RAT;
            Name = "Rat";
            Labels.AddRange(new string[] { "rat", "animal", "vermin", "it" });
            Description = "There is a RAT in the corner. It seems to be comfortable and not at all afraid of you.";
            KillBy.Add(ItemType.WEAPON_MELEE);
            KillBy.Add(ItemType.FIST);
            ShowDescriptionWhenDead = false;
            DropItem = new PassedOutRat();
        }
    }
}
