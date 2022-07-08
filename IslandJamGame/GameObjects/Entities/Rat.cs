using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class Rat : Entity
    {
        public Rat()
        {
            Id = Id.ENTITY_BUNGALOW_RAT;
            Name = "Rat";
            Labels = new string[] { "Rat", "Animal", "Vermin", "It" };
            Description = "There is a RAT in the corner. It seems to be comfortable and not at all afraid of you.";
            KillBy.Add(ItemType.MELEE);
            ShowDescriptionWhenDead = false;
            DropItem = new PassedOutRat();
        }
    }
}
