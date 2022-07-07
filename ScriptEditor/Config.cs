namespace ScriptEditor
{
    public class Window
    {
        public int Width { get; set; } = 1024;
        public int Height { get; set; } = 800;
        public int X { get; set; } = 100;
        public int Y { get; set; } = 100;
    }

    public class Config
    {
        public string LastFile { get; set; } = "";
        public string ProjectFolder { get; set; } = "";
        public string ProjectDebugFilename { get; set; } = "";
        public string MSBuildFilename { get; set; } = "";
        public Window Window { get; set; } = new Window();
    }
}
