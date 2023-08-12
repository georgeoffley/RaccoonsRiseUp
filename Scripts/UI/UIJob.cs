namespace RRU;

public partial class UIJob : Node
{
    [Export] GameState gameState;

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

    JobType job;

    public override void _Ready()
    {
        btnPlus.Pressed += OnAddPressed;
        btnMinus.Pressed += OnRemovePressed;

        UpdateCountLabel();
    }

    void UpdateCountLabel()
    {
        labelCount.Text = gameState.Jobs[job].ToString();
    }

    /// Event Handlers ///

    void OnAddPressed()
    {
        if (!gameState.AddJob(job))
            return;

        UpdateCountLabel();
        RaccoonAssigned.Invoke(job);
    }

    void OnRemovePressed()
    {
        if (!gameState.RemoveJob(job))
            return;

        UpdateCountLabel();
        RaccoonUnassigned.Invoke(job);
    }
}
