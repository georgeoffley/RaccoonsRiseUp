namespace RRU;

public partial class UIJob : Node
{
    [Export] Label labelName;
    [Export] Label labelCount;
    [Export] Button btnMinus;
    [Export] Button btnPlus;

    public static event Action<JobType> RaccoonAssigned;
    public static event Action<JobType> RaccoonUnassigned;

    public JobType Job
    {
        get => job;
        set
        {
            job = value;
            labelName.Text = value.ToString();
        }
    }

    public int CurEmployedRaccoons
    {
        get => int.Parse(labelCount.Text);
        set => labelCount.Text = value.ToString();
    }

    JobType job;

    public override void _Ready()
    {
        btnMinus.Pressed += () =>
        {
            // There are no more raccoons to take away from this job
            if (CurEmployedRaccoons <= 0)
                return;

            // Raccoon is no longer employed
            CurEmployedRaccoons--;
            RaccoonUnassigned?.Invoke(job);
        };

        btnPlus.Pressed += () =>
        {
            // No more raccoons are available to get a job
            if (Game.Raccoons <= 0)
                return;

            // Raccoon assigned to job
            CurEmployedRaccoons++;
            RaccoonAssigned?.Invoke(job);
        };
    }
}
