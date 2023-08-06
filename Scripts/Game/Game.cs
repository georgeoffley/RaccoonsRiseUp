namespace RRU;

public partial class Game : Node
{
    public static int Raccoons { get; set; } = 30;
    public static event Action<Dictionary<ResourceType, double>> ResourcesChanged;

    [Export] UIInfo pageInfo;
    [Export] UIJobs pageJobs;

    Dictionary<JobType, int> numJobs = new()
    {
        { JobType.Woodcutter, 0 },
        { JobType.Researcher, 0 }
    };

    Dictionary<ResourceType, double> numResources = new()
    {
        { ResourceType.Wood, 0 },
        { ResourceType.Tech, 0 }
    };

    Dictionary<StructureType, int> numStructures = new()
    {
        { StructureType.LumberCamp, 0 },
        { StructureType.ResearchCamp, 0 }
    };

    public override void _Ready()
    {
        pageInfo.Raccoons = Raccoons;
        pageJobs.Raccoons = Raccoons;

        UIJob.RaccoonAssigned += job =>
        {
            numJobs[job]++;
            Raccoons--;
        };

        UIJob.RaccoonUnassigned += job =>
        {
            numJobs[job]--;
            Raccoons++;
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        var resourcesChanged = false;

        ResourcesGainedByStructures(delta, ref resourcesChanged);
        ResourcesGainedByJobs(delta, ref resourcesChanged);

        if (resourcesChanged)
            ResourcesChanged?.Invoke(numResources);
    }

    void ResourcesGainedByStructures(double delta, ref bool resourcesChanged)
    {
        foreach (var structure in structureData)
        {
            var structureData = structure.Value;

            if (numStructures[structure.Key] == 0)
                continue;

            structureData.Resources.ForEach(x => x.Value.ElpasedTime += delta);

            foreach (var resource in structureData.Resources)
            {
                var resourceData = resource.Value;
                if (resourceData.ElpasedTime >= resourceData.GatherRate)
                {
                    var timesEarned = resourceData.ElpasedTime / resourceData.GatherRate;

                    resourceData.ElpasedTime -= resourceData.GatherRate * timesEarned;
                    numResources[resource.Key] += resourceData.GatherAmount * timesEarned * numStructures[structure.Key];
                    resourcesChanged = true;
                }
            }
        }
    }

    void ResourcesGainedByJobs(double delta, ref bool resourcesChanged)
    {
        foreach (var job in jobData)
        {
            var jobData = job.Value;

            if (numJobs[job.Key] == 0)
                continue;

            jobData.ElpasedTime += delta;

            if (jobData.ElpasedTime >= jobData.GatherRate)
            {
                var timesEarned = jobData.ElpasedTime / jobData.GatherRate;

                jobData.ElpasedTime -= jobData.GatherRate * timesEarned;
                numResources[jobData.ResourceType] += jobData.GatherAmount * timesEarned * numJobs[job.Key];
                resourcesChanged = true;
            }
        }
    }
}
