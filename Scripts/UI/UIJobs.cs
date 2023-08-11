namespace RRU;

public partial class UIJobs : Node
{
    [Export] GameState gameState;

    [Export] Label labelRaccoons;
    [Export] GridContainer grid;

    public int Raccoons
    {
        get => int.Parse(labelRaccoons.Text);
        set => labelRaccoons.Text = value.ToString();
    }

    public override void _Ready()
    {
        Raccoons = gameState.Raccoons;

        UIJob.RaccoonAssigned += job =>
        {
            Raccoons--;
        };

        UIJob.RaccoonUnassigned += job =>
        {
            Raccoons++;
        };

        ReadOnlySpan<JobType> jobs = default;
        gameState.GetJobTypes(ref jobs);

        for (int i = 0; i < jobs.Length; ++ i)
        {
            AddJob(jobs[i]);
        }
    }

    void AddJob(JobType job)
    {
        var jobPrefab = (UIJob)Prefabs.Job.Instantiate();

        jobPrefab.Job = job;
        grid.AddChild(jobPrefab);
    }
}
