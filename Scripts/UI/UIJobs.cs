namespace Template;

public partial class UIJobs : Node
{
    GridContainer grid;

    public override void _Ready()
    {
        grid = GetNode<GridContainer>("GridContainer");

        AddJob("Woodcutter");
        AddJob("Researcher");
    }

    void AddJob(string name)
    {
        var job = (UIJob)Prefabs.Job.Instantiate();
        job.JobName = name;
        grid.AddChild(job);
    }
}
