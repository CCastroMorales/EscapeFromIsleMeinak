namespace MeinakEsc
{
    public struct InputBundle
    {
        public string Command { get; set; }
        public string[] Arguments { get; set; }
        public string FirstArgument { get => Arguments[0]; }
        public bool HasArguments { get => Arguments != null && Arguments.Length > 0; }

        public InputBundle(string command, string[] arguments)
        {
            Command = command;
            Arguments = arguments;
        }
    }
}
