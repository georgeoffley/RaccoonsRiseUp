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

public class JobData
{
    public ResourceType ResourceType { get; set; }
    public double ElpasedTime { get; set; }
    public double GatherRate { get; set; }
    public double GatherAmount { get; set; }
}

public enum JobType
{
    Woodcutter,
    Researcher
}
