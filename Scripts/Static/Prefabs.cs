namespace RRU;

public static class Prefabs
{
    public static PackedScene Popup { get; } = Load("UI/popup");
    public static PackedScene TechNode { get; } = Load("UI/tech_node");
    public static PackedScene Job { get; } = Load("UI/job");
    public static PackedScene Options { get; } = Load("UI/options");

    static PackedScene Load(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/{path}.tscn");
}
