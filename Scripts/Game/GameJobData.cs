namespace RRU;

public partial class Game
{
    Dictionary<JobType, JobData> jobData = new()
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
}
