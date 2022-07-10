using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
{
    public class Bed : CheckObject
    {
        public Bed()
        {
            Id = Id.CHECK_OBJECT_BED;
            Name = "Bed";
            Labels.Add("bed");
            Description = "You see a silhouette that resembles your friend on the bed. You touch your friend gently but realize that it was all an illusion. It's just the cover and pillows. Your friend is not here.";
            
        }
    }
}
