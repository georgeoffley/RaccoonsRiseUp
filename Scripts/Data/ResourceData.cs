namespace RRU;

public class ResourceData
{
    public double ElpasedTime { get; set; }
    public double GatherRate { get; set; }
    public double GatherAmount { get; set; }
}

public enum ResourceType
{
    Wood,
    Tech
}
