namespace RRU;

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
