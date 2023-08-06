namespace RRU;

public class StructureData
{
    public Dictionary<ResourceType, ResourceData> Resources { get; set; } = new();
}

public enum StructureType
{
    LumberCamp,
    ResearchCamp
}
