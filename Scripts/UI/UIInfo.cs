namespace RRU;

public partial class UIInfo : Node
{
    [Export] Label labelRaccoons;
    [Export] Label labelWoodcutters;
    [Export] Label labelResearchers;
    [Export] Label labelWood;
    [Export] Label labelTech;

    public int Raccoons
    {
        get => int.Parse(labelRaccoons.Text);
        set => labelRaccoons.Text = value.ToString();
    }

    Dictionary<JobType, Label> jobs;

    public override void _Ready()
    {
        jobs = new()
        {
            { JobType.Woodcutter, labelWoodcutters },
            { JobType.Researcher, labelResearchers }
        };

        Game.ResourcesChanged += resources =>
        {
            labelWood.Text = Mathf.Round(resources[ResourceType.Wood]).ToString();
            labelTech.Text = Mathf.Round(resources[ResourceType.Tech]).ToString();
        };

        Game.JobsChanged += jobs =>
        {
            labelWoodcutters.Text = jobs[JobType.Woodcutter].ToString();
            labelResearchers.Text = jobs[JobType.Researcher].ToString();
        };

        UIJob.RaccoonAssigned += job =>
        {
            var count = int.Parse(jobs[job].Text);
            count++;
            jobs[job].Text = count.ToString();

            Raccoons--;
        };

        UIJob.RaccoonUnassigned += job =>
        {
            var count = int.Parse(jobs[job].Text);
            count--;
            jobs[job].Text = count.ToString();

            Raccoons++;
        };
    }
}
