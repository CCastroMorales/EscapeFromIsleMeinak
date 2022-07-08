using IslandJamGame.Engine;

namespace IslandJamGame.GameObjects
{
    public class Note : Item
    {
        public Note()
        {
            Id = Id.ITEM_NOTE;
            Name = "Note";
            Description = "There appears to be a wrinkled NOTE in the trash can.";
            InventoryDescription = "The note is from your friend; you recognize their sloppy handwriting.";
            // TODO Add actions
            Labels.Add("note");
        }
    }
}
