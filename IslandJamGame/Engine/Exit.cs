using System.Collections.Generic;

namespace IslandJamGame.Engine
{
    public class Exit
    {
        public Id Destination { get; set; }
        public List<string> Commands { get; set; } = new List<string>();
        public string TriggerEntityId { get; set; } = "";
    }
}
