namespace Template;

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

    Dictionary<Job, Label> jobs;

    public override void _Ready()
    {
        jobs = new()
        {
            { Job.Woodcutter, labelWoodcutters },
            { Job.Researcher, labelResearchers }
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
