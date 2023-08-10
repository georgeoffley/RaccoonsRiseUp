namespace RRU;

/// <summary>
/// A TechInfo represents an instance of TechData with its own modifiers
/// </summary>
public sealed partial class TechInfo : RefCounted
{
    public StringName Id { get; private set; }
    public float Modifier { get; private set; }

    public TechData Data { get; private set; }
    public TechType Type { get; private set; }

    /// <summary>
    /// Creates a Godot-compatible info object from a given type
    /// </summary>
    /// <returns></returns>
    public static TechInfo FromType(StringName id, float modifier, TechType type)
    {
        TechInfo info = new()
        {
            Id = id,
            Modifier = modifier,
            Type = type
        };

        ReadOnlySpan<TechType> types = Game.TechData.Keys.ToArray();

        for (int i = 0; i < types.Length; ++ i)
        {
            if (types[i] != type)
                continue;

            info.Data = Game.TechData[types[i]];
        }

        return info;
    }
}
