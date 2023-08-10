namespace RRU;

/// <summary>
/// A TechInfo represents an instance of TechData with its own modifiers
/// </summary>

// RefCounted can be used with Godot methods, unlike regular C# 'object'
// classes. (e.g. you can use it in signals.)
public sealed partial class TechInfo : RefCounted
{
    public StringName Id { get; private set; }
    public float Modifier { get; private set; }

    public TechData Data { get; private set; }
    public TechType Type { get; private set; }

    /// <summary>
    /// Creates a Godot-compatible info object from a given type.
    /// This function finds a matching 'GameData' from the 'Game.TechData' 
    /// dictionary.
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

        // Casting to ReadOnlySpan<TechType> because based on previous
        // experience, Iterating on spans are a lot faster than other
        // collection types. Plus you get to use the standard C-style
        // iterator since you're not dealing with an enumerator anymore.
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
