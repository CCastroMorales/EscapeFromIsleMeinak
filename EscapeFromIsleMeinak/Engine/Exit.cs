using System.Collections.Generic;

namespace EscapeFromIsleMainak.Engine
{
    public class Exit
    {
        public Id Destination { get; set; }
        public List<string> Commands { get; set; } = new List<string>();
        public Id TriggerEntityId { get; set; }
    }
}
