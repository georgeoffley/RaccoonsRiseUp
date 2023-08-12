namespace RRU;

public partial class UIJobs : Node
{
    [Export] GameState gameState;

    [Export] Label labelRaccoons;
    [Export] GridContainer grid;

    public override void _Ready()
    {
        ReadOnlySpan<JobType> jobs = default;
        gameState.GetJobTypes(ref jobs);

        for (int i = 0; i < jobs.Length; ++i)
            AddJob(jobs[i]);

        UIJob.RaccoonAssigned   += _ => UpdateRaccoonsLabel();
        UIJob.RaccoonUnassigned += _ => UpdateRaccoonsLabel();

        UpdateRaccoonsLabel();
    }

    void AddJob(JobType job)
    {
        var jobPrefab = (UIJob)Prefabs.Job.Instantiate();

        jobPrefab.Job = job;
        grid.AddChild(jobPrefab);
    }

    void UpdateRaccoonsLabel()
    {
        labelRaccoons.Text = gameState.Raccoons.ToString();
    }
}
