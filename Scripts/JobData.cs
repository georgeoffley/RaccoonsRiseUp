namespace Template;

public class JobData
{
    public int NumWorkers { get; set; }
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
