namespace RRU;

[GlobalClass]
public sealed partial class TechDataService : Resource
{
    const int MaxUpgrades = 128;

    [Signal]
    public delegate void ResearchStateUpdatedEventHandler(TechDataService service, StringName id, bool isResearched);

    // Note that renaming this will cause the resource to get reset
    // Not worth the hassle
    [Export] TechUpgradeInfo[] _upgrades;

    readonly HashSet<StringName> researchedUpgrades;

    public TechDataService()
    {
        researchedUpgrades = new();
    }

    /// Upgrades ///

    public void Research(StringName id)
    {
        researchedUpgrades.Add(id);
        EmitSignal(SignalName.ResearchStateUpdated, this, id, true);
    }

    public void Unresearch(StringName id)
    {
        if (!researchedUpgrades.Contains(id))
            return;

        researchedUpgrades.Remove(id);
        EmitSignal(SignalName.ResearchStateUpdated, this, id, false);
    }

    public bool IsResearched(StringName id) => 
        researchedUpgrades.Contains(id);

    public bool IsUnlocked(StringName id)
    {
        if (id == null)
            return false;

        TechUpgradeInfo info = GetInfoForId(id);

        Debug.Assert(
            condition: info != null,
            message: $"Invalid upgrade ID: {id}"
        );

        ReadOnlySpan<string> prerequisiteIds = info.RequiredUpgradeIds;

        for (int i = 0; i < prerequisiteIds.Length; ++ i)
        {
            if (IsResearched(prerequisiteIds[i]))
                continue;

            return false;
        }

        return true;
    }

    /// <summary>
    /// Writes to a span pointing to a list of all available tech upgrades.
    /// </summary>
    public void GetAllUpgrades(ref ReadOnlySpan<TechUpgradeInfo> upgrades)
    {
        upgrades = _upgrades;
    }

    public TechUpgradeInfo GetInfoForId(StringName id)
    {
        ReadOnlySpan<TechUpgradeInfo> upgrades = _upgrades;

        for (int i = 0; i < upgrades.Length; ++ i)
        {
            if (upgrades[i].Id != id)
                continue;

            return upgrades[i];
        }

        return null;
    }

    public void Reset() => researchedUpgrades.Clear();

    /// Serialisation ///

    /// <summary>
    /// Returns an array containing all currently-researched upgrade options.
    /// </summary>
    /// <returns></returns>
    public string[] Serialise()
    {
        Span<string> upgrades = new string[MaxUpgrades];
        int upgradeIdx = 0;

        foreach (StringName id in researchedUpgrades)
        {
            upgrades[upgradeIdx] = id;
            upgradeIdx ++;
        }

        return upgrades[..upgradeIdx].ToArray();
    }

    public void Deserialise(string[] serialisedArray)
    {
        ReadOnlySpan<string> ids = serialisedArray;

        researchedUpgrades.Clear();

        for (int i = 0; i < ids.Length; ++ i)
        {
            researchedUpgrades.Add(ids[i]);
        }
    }
}
