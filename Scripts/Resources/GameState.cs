namespace RRU;

/// <summary>
/// This resource's purpose is to allow communication between various parts of the
/// application without needing direct references to one another.
/// </summary>
[GlobalClass]
public sealed partial class GameState : Resource
{
    public event Action<ResourceDict> ResourcesChanged;
    public event Action<JobDict> JobsChanged;

    [Export] TechDataService dataService;
    [Export] public JobData[] JobData;

    public int Raccoons { get; set; } = 30;

    public JobDict Jobs { get; set; }
    public StructureDict Structures { get; set; }
    public ResourceDict Resources { get; set; }

    readonly JobType[] jobTypes;
    readonly ResourceType[] resourceTypes;
    readonly StructureType[] structureTypes;

    public GameState()
    {
        Jobs = new();
        Resources = new();
        Structures = new();

        // Performance cost should be negligible since this should only be called once.
        // Using this should be more future-proof
        // as the previous implementation requires someone to update each dictionary
        // whenever a new type is added.

        jobTypes = (JobType[]) Enum.GetValues(typeof(JobType));
        resourceTypes = (ResourceType[]) Enum.GetValues(typeof(ResourceType));
        structureTypes = (StructureType[]) Enum.GetValues(typeof(StructureType));

        // Initialise everything in a single pass
        int l = Math.Max(
            jobTypes.Length,
            Math.Max(resourceTypes.Length, structureTypes.Length)
        );

        for (int i = 0; i < l; ++i)
        {
            if (i < jobTypes.Length)
            {
                Jobs[jobTypes[i]] = 0;
            }

            if (i < resourceTypes.Length)
            {
                Resources[resourceTypes[i]] = 0;
            }

            if (i < structureTypes.Length)
            {
                Structures[structureTypes[i]] = 0;
            }
        }
    }

    public void UpdateResources()
    {
        ResourcesChanged?.Invoke(Resources);
    }

    public void UpdateJobs()
    {
        JobsChanged?.Invoke(Jobs);
    }

    /// Resources ///

    public void GetResourceTypes(ref ReadOnlySpan<ResourceType> types)
    {
        types = resourceTypes;
    }

    public bool HasResource(ResourceType type, double amount)
    {
        if (!Resources.ContainsKey(type))
            return false;

        return Resources[type] >= amount;
    }

    /// Jobs ///

    public void GetJobTypes(ref ReadOnlySpan<JobType> types)
    {
        types = jobTypes;
    }

    public bool AddJob(JobType job)
    {
        if (Raccoons <= 0)
            return false;

        Jobs[job]++;
        Raccoons--;

        return true;
    }

    public bool RemoveJob(JobType job)
    {
        if (Jobs[job] <= 0)
            return false;

        Jobs[job]--;
        Raccoons++;

        return true;
    }

    /// Structures ///

    public void GetStructureTypes(ref ReadOnlySpan<StructureType> types)
    {
        types = structureTypes;
    }

    /// Upgrades ///

    public bool CanPurchaseUpgrade(TechUpgradeInfo upgrade)
    {
        if (upgrade == null)
            return false;

        // Check if all the required materials are present in the inventory
        ReadOnlySpan<ResourceRequirement> requirements = upgrade.UpgradeCost;

        for (int i = 0; i < requirements.Length; ++i)
        {
            if (HasResource(requirements[i].Type, requirements[i].Amount))
                continue;

            return false;
        }

        return true;
    }

    public void ConsumeUpgradeMaterials(TechUpgradeInfo upgrade)
    {
        if (upgrade == null)
            return;

        ReadOnlySpan<ResourceRequirement> requirements = upgrade.UpgradeCost;

        for (int i = 0; i < requirements.Length; ++i)
        {
            Resources[requirements[i].Type] -= requirements[i].Amount;
        }

        UpdateResources();
    }
}
