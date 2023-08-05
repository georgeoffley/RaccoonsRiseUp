namespace Template;

public partial class UIJobs : Node
{
    [Export] GridContainer grid;

    public override void _Ready()
    {
        AddJob(Job.Woodcutter);
        AddJob(Job.Researcher);
    }

    void AddJob(Job job)
    {
        var jobPrefab = (UIJob)Prefabs.Job.Instantiate();
        jobPrefab.Job = job;
        grid.AddChild(jobPrefab);
    }
}
