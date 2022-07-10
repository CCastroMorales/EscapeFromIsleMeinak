using System.Collections.Generic;

namespace EscapeFromIsleMainak.Components
{
    public class CheckObject
    {
        public Id Id { get; set; }
        public string Name { get; set; } = "";
        public List<string> Labels { get; set; } = new List<string>();
        public string Description { get; set; } = "";
    }
}
