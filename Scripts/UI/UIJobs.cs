namespace RRU;

public partial class UIJobs : Node
{
    [Export] Label labelRaccoons;
    [Export] GridContainer grid;

    public int Raccoons
    {
        get => int.Parse(labelRaccoons.Text);
        set => labelRaccoons.Text = value.ToString();
    }

    public override void _Ready()
    {
        AddJob(JobType.Woodcutter);
        AddJob(JobType.Researcher);

        UIJob.RaccoonAssigned += job =>
        {
            Raccoons--;
        };

        UIJob.RaccoonUnassigned += job =>
        {
            Raccoons++;
        };
    }

    void AddJob(JobType job)
    {
        var jobPrefab = (UIJob)Prefabs.Job.Instantiate();
        jobPrefab.Job = job;
        grid.AddChild(jobPrefab);
    }
}
