namespace RRU;

public sealed partial class TechNodeInfo: RefCounted
{
	public TechData Data;
	public TechType Type;

	/// <summary>
	/// Creates a Godot-compatible info object from a given type
	/// </summary>
	/// <returns></returns>
	public static TechNodeInfo FromType(TechType type)
	{
		TechNodeInfo info = new()
		{
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