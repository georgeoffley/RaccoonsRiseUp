namespace RRU;

public class StructureData
{
    public int NumStructures { get; set; }
    public Dictionary<ResourceType, ResourceData> Resources { get; set; } = new();
}

public enum StructureType
{
    LumberCamp,
    ResearchCamp
}
