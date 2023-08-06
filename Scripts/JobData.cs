namespace RRU;

public partial class JobData : Resource
{
    [Export] public JobType JobType { get; set; }
    [Export] public ResourceType ResourceType { get; set; }
    [Export] public double GatherRate { get; set; }
    [Export] public double GatherAmount { get; set; }

    public double ElpasedTime { get; set; }
}

public enum JobType
{
    Woodcutter,
    Researcher
}
