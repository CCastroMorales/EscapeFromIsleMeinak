using MeinakEsc.Components;

namespace EscapeFromIsleMeinak.GameObjects
{
    public class Motor : CheckObject
    {
        public Motor()
        {
            Id = Id.CHECK_OBJECT_MOTOR;
            Name = "Motor";
            Description = "You open the hood and look at the motor and wonder why you're pretending to be a mechanic. You don't know what you're looking at and close the hood again.";
            Labels.AddRange(new string[] { "motor", "engine" });
        }
    }
}
