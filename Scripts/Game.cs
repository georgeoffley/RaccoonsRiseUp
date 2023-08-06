namespace Template;

public partial class Game : Node
{
    public static int Raccoons { get; set; } = 3;

    [Export] UIInfo pageInfo;
    [Export] UIJobs pageJobs;

    Dictionary<Job, int> jobs = new()
    {
        { Job.Woodcutter, 0 },
        { Job.Researcher, 0 }
    };

    public override void _Ready()
    {
        pageInfo.Raccoons = Raccoons;
        pageJobs.Raccoons = Raccoons;

        UIJob.RaccoonAssigned += job =>
        {
            jobs[job]++;
            Raccoons--;
        };

        UIJob.RaccoonUnassigned += job =>
        {
            jobs[job]--;
            Raccoons++;
        };
    }
}

public enum Job
{
    Woodcutter,
    Researcher
}
