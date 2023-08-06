namespace Template;

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
        AddJob(Job.Woodcutter);
        AddJob(Job.Researcher);

        UIJob.RaccoonAssigned += job =>
        {
            Raccoons--;
        };

        UIJob.RaccoonUnassigned += job =>
        {
            Raccoons++;
        };
    }

    void AddJob(Job job)
    {
        var jobPrefab = (UIJob)Prefabs.Job.Instantiate();
        jobPrefab.Job = job;
        grid.AddChild(jobPrefab);
    }
}
