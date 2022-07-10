using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
{
    public class Note : Item
    {
        public Note()
        {
            Id = Id.ITEM_NOTE;
            Name = "Note";
            InventoryLabel = "Note";
            Description = "There appears to be a wrinkled NOTE in the trash can.";
            InventoryDescription = "The note is from your friend; you recognize their sloppy handwriting.";
            Labels.Add("note");
            Actions.Add(new ItemAction(Id.ACTION_READ, "The note reads: \"What is this feeling? I shouldn't have gone near that person at the party. I guess I'll I have no choice than to be honest w ht\"... The note ends abruptly."));
        }
    }
}
