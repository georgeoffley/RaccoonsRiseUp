namespace Template;

public partial class Game : Node
{
    public static int Raccoons { get; set; } = 3;

    public static int Woodcutters { get; set; }
    public static int Researchers { get; set; }
}

public enum Job
{
    Woodcutter,
    Researcher
}
