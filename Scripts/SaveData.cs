namespace RRU;

public partial class SaveData
{
    public int Raccoons { get; set; }
    public Dictionary<JobType, int> NumJobs { get; set; }
    public Dictionary<ResourceType, double> NumResources { get; set; }
    public Dictionary<StructureType, int> NumStructures { get; set; }
}
