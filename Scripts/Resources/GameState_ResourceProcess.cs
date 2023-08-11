namespace RRU;

public sealed partial class GameState
{
    // Change this depending on the needs of the project, but this should be enough for the time being
    // Consider increasing it if (tech upgrades count + job type count + structure type count) > current modifier limit
    const int MAX_MODIFIERS = 128;

    public void ProcessResourceTick(double delta)
    {
        Span<IResourceModifier> modifiers = new IResourceModifier[MAX_MODIFIERS];
        int modifierIdx = 0;

        // Add job + structure modifiers
        ReadOnlySpan<JobData> jobs = JobData;

        for (int i = 0; i < jobs.Length; ++ i)
        {
            AppendToSpan(jobs[i], ref modifiers, ref modifierIdx);
        }

        // TODO: Add structure-related instructions once it is implemented

        // Add upgrade modifiers
        Span<TechUpgradeInfo> techUpgrades = default;
        dataService.GetResearchedUpgrades(ref techUpgrades);

        for (int i = 0; i < techUpgrades.Length; ++ i)
        {
            ReadOnlySpan<ResourceModifierDefinition> modifierDefs = techUpgrades[i].Modifiers;

            for (int j = 0; j < modifierDefs.Length; ++ j)
            {
                AppendToSpan(modifierDefs[j], ref modifiers, ref modifierIdx);
            }
        }

        modifiers = modifiers[..modifierIdx];
        EvaluateModifiers(modifiers, delta);
    }

    void EvaluateModifiers(Span<IResourceModifier> modifiers, double delta)
    {
        ReadOnlySpan<ResourceType> resourceTypes = default;
        GetResourceTypes(ref resourceTypes);

        // Multiplier pass //

        Span<ResourceMultiplier> multipliers =
            stackalloc ResourceMultiplier[resourceTypes.Length];

        // Set default multipliers
        for (int i = 0; i < resourceTypes.Length; ++ i)
        {
            multipliers[i] = new(
                resource: resourceTypes[i],
                multiplier: 1.0
            );
        }

        // Accumulate modifiers from all sources
        for (int i = 0; i < modifiers.Length; ++ i)
        {
            if (!modifiers[i].ModifierIsActive(this, delta))
                continue;

            ResourceModifier modifier = default;
            modifiers[i].ModifierGet(this, ref modifier);

            if (modifier.Type != ResourceModifierType.Multiplicative)
                continue;

            for (int j = 0; j < multipliers.Length; ++ j)
            {
                if (multipliers[j].Resource != modifier.Resource)
                    continue;

                multipliers[j].Multiplier += modifier.Amount;
                break;
            }
        }

        // Additive Pass //

        for (int i = 0; i < modifiers.Length; ++ i)
        {
            if (!modifiers[i].ModifierIsActive(this, delta))
                continue;

            ResourceModifier modifier = default;
            modifiers[i].ModifierGet(this, ref modifier);

            if (modifier.Type != ResourceModifierType.Additive)
                continue;

            double multiplier = 1.0;

            for (int j = 0; j < multipliers.Length; ++ j)
            {
                if (modifier.Resource != multipliers[j].Resource)
                    continue;

                multiplier = multipliers[j].Multiplier;
                break;
            }

            double modTotal = modifier.Amount * multiplier;

            Resources[modifier.Resource] =
                Mathf.Max(0.0f, Resources[modifier.Resource] + modTotal);
        }

        UpdateResources();
    }

    void AppendToSpan(
        IResourceModifier modifier,
        ref Span<IResourceModifier> modifiers,
        ref int index)
    {
        modifiers[index] = modifier;
        index ++;
    }

    struct ResourceMultiplier
    {
        public ResourceType Resource { get; private set; }
        public double Multiplier { get; set; }

        public ResourceMultiplier(ResourceType resource, double multiplier)
        {
            Resource = resource;
            Multiplier = multiplier;
        }
    }
}

/// <summary>
/// This interface must be implemented, if one wishes to make changes to the game state's resources.
/// </summary>
public interface IResourceModifier
{
    public bool ModifierIsActive(GameState context, double delta);
    public void ModifierGet(GameState context, ref ResourceModifier info);
}

public enum ResourceModifierType
{
    Additive,
    Multiplicative
}

public ref struct ResourceModifier
{
    public readonly ResourceType Resource;
    public readonly ResourceModifierType Type;
    public readonly double Amount;

    public ResourceModifier(
        ResourceType resource,
        ResourceModifierType type,
        double amount)
    {
        Resource = resource;
        Type = type;
        Amount = amount;
    }
}