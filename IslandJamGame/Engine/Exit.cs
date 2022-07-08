namespace IslandJamGame.Engine
{
    public class Exit
    {
        public Id Destination { get; set; }
        public string Command { get; set; } = "";
        public string TriggerEntityId { get; set; } = "";
    }
}
