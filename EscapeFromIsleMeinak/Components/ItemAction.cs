namespace EscapeFromIsleMeinak.Components
{
    public class ItemAction
    {
        public Id Id { get; set; }
        public string Text { get; set; } = "";

        public ItemAction(Id id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
