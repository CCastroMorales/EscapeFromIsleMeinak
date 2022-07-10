using MeinakEsc.Components;

namespace MeinakEsc.GameObjects
{
    public class Compartment : CheckObject
    {
        public Compartment()
        {
            Id = Id.CHECK_OBJECT_COMPARTMENT;
            Name = "Compartment";
            Description = "You peek inside the compartment. Hm, there's not much in there. ITEM_DESCRIPTIONS";
            Labels.AddRange(new string[] { "compartment", "box" });
        }
    }
}
