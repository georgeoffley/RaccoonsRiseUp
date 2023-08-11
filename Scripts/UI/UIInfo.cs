namespace RRU;

public partial class UIInfo : Node
{
    [Export] GameState gameState;

    [Export] Label labelRaccoons;
    [Export] Label labelWoodcutters;
    [Export] Label labelResearchers;
    [Export] Label labelWood;
    [Export] Label labelTech;

    public int Raccoons
    {
        get => int.Parse(labelRaccoons.Text);
        set => labelRaccoons.Text = value.ToString();
    }

    Dictionary<JobType, Label> jobs;
    Dictionary<ResourceType, Label> resources;

    public override void _Ready()
    {
        Raccoons = gameState.Raccoons;

        jobs = new()
        {
            { JobType.Woodcutter, labelWoodcutters },
            { JobType.Researcher, labelResearchers }
        };

        resources = new()
        {
            [ResourceType.Wood] = labelWood,
            [ResourceType.Tech] = labelTech
        };

        gameState.ResourcesChanged += _ => UpdateResourceCounts();
        gameState.JobsChanged += _ => UpdateAllJobCounts();

        UIJob.RaccoonAssigned += UpdateJobCount;
        UIJob.RaccoonUnassigned += UpdateJobCount;

        // Initial update
        UpdateResourceCounts();
        UpdateAllJobCounts();
    }

    /// Event Handlers ///

    void UpdateResourceCounts()
    {
        ReadOnlySpan<ResourceType> resourceTypes = default;
        gameState.GetResourceTypes(ref resourceTypes);

        for (int i = 0; i < resourceTypes.Length; ++ i)
        {
            ResourceType type = resourceTypes[i];

            if (!resources.TryGetValue(type, out Label label))
                continue;

            label.Text = $"{gameState.Resources[type]:0.00}";
        }
    }

    void UpdateAllJobCounts()
    {
        ReadOnlySpan<JobType> jobs = default;
        gameState.GetJobTypes(ref jobs);

        for (int i = 0; i < jobs.Length; ++ i)
        {
            UpdateJobCount(jobs[i]);
        }
    }

    void UpdateJobCount(JobType job)
    {
        if (!jobs.TryGetValue(job, out Label label))
            return;

        label.Text = gameState.Jobs[job].ToString();
    }
}
