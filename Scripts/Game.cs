namespace RRU;

public partial class Game : Node
{
    public static int Raccoons { get; set; } = 30;
    public static event Action<Dictionary<ResourceType, double>> ResourcesChanged;

    [Export] UIInfo pageInfo;
    [Export] UIJobs pageJobs;

    Dictionary<JobType, JobData> jobs = new()
    {
        { 
            JobType.Woodcutter, new JobData 
            {
                ResourceType = ResourceType.Wood,
                GatherRate = 1,
                GatherAmount = 1
            }
        },
        {
            JobType.Researcher, new JobData
            {
                ResourceType = ResourceType.Tech,
                GatherRate = 1,
                GatherAmount = 1
            }
        }
    };

    Dictionary<ResourceType, double> resources = new()
    {
        { ResourceType.Wood, 0 },
        { ResourceType.Tech, 0 }
    };

    Dictionary<StructureType, StructureData> structures = new()
    {
        { 
            StructureType.LumberCamp, new StructureData 
            {
                NumStructures = 0,
                Resources = new()
                {
                    { 
                        ResourceType.Wood, new ResourceData 
                        {
                            GatherRate = 0.1,
                            GatherAmount = 1
                        }
                    }
                }
            }
        },
        {
            StructureType.ResearchCamp, new StructureData
            {
                NumStructures = 0,
                Resources = new()
                {
                    {
                        ResourceType.Tech, new ResourceData
                        {
                            GatherRate = 0.5,
                            GatherAmount = 1
                        }
                    }
                }
            }
        }
    };

    public override void _Ready()
    {
        pageInfo.Raccoons = Raccoons;
        pageJobs.Raccoons = Raccoons;

        UIJob.RaccoonAssigned += job =>
        {
            jobs[job].NumWorkers++;
            Raccoons--;
        };

        UIJob.RaccoonUnassigned += job =>
        {
            jobs[job].NumWorkers--;
            Raccoons++;
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        var resourcesChanged = false;

        ResourcesGainedByStructures(delta, ref resourcesChanged);
        ResourcesGainedByJobs(delta, ref resourcesChanged);

        if (resourcesChanged)
            ResourcesChanged?.Invoke(resources);
    }

    void ResourcesGainedByStructures(double delta, ref bool resourcesChanged)
    {
        foreach (var structure in structures)
        {
            var structureData = structure.Value;

            if (structureData.NumStructures == 0)
                continue;

            structureData.Resources.ForEach(x => x.Value.ElpasedTime += delta);

            foreach (var resource in structureData.Resources)
            {
                var resourceData = resource.Value;
                if (resourceData.ElpasedTime >= resourceData.GatherRate)
                {
                    var timesEarned = resourceData.ElpasedTime / resourceData.GatherRate;

                    resourceData.ElpasedTime -= resourceData.GatherRate * timesEarned;
                    resources[resource.Key] += timesEarned * structureData.NumStructures;
                    resourcesChanged = true;
                }
            }
        }
    }

    void ResourcesGainedByJobs(double delta, ref bool resourcesChanged)
    {
        foreach (var job in jobs)
        {
            var jobData = job.Value;

            if (jobData.NumWorkers == 0)
                continue;

            jobData.ElpasedTime += delta;

            if (jobData.ElpasedTime >= jobData.GatherRate)
            {
                var timesEarned = jobData.ElpasedTime / jobData.GatherRate;

                jobData.ElpasedTime -= jobData.GatherRate * timesEarned;
                resources[jobData.ResourceType] += timesEarned * jobData.NumWorkers;
                resourcesChanged = true;
            }
        }
    }
}
