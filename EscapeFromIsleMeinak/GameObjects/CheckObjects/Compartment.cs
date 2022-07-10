using EscapeFromIsleMainak.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class Compartment : CheckObject
    {
        public Compartment()
        {
            Id = Id.CHECK_OBJECT_COMPARTMENT;
            Name = "Compartment";
            Description = "You peek inside the compartment. There's nothing there.";
            Labels.AddRange(new string[] { "compartment", "box" });
        }
    }
}
