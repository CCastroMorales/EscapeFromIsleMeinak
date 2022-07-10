using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class Mirror : CheckObject
    {
        public Mirror()
        {
            Id = Id.CHECK_OBJECT_MIRROR;
            Name = "Mirror";
            Description = "Is the message on the mirror written with lipstick? The full moon light allows you to barely read what it says: \"The meinak... it is real. i saw it floating head. it speak to me. If you read dis escape!\". You look out through the small window and see a strange orb moving slowly in the forest.";
            Labels.AddRange(new string[] { "mirror", "message" });
        }
    }
}
