namespace Template;

public partial class UIJob : Node
{
    [Export] Label labelName;
    [Export] Label labelCount;
    [Export] Button btnMinus;
    [Export] Button btnPlus;

    public Job Job
    {
        get => job;
        set
        {
            job = value;
            labelName.Text = value + "";
        }
    }

    public int Count
    {
        get => int.Parse(labelCount.Text);
        set
        {
            switch (job)
            {
                case Job.Woodcutter:
                    Game.Woodcutters = value;
                    break;
                case Job.Researcher:
                    Game.Researchers = value;
                    break;
            }

            labelCount.Text = value + "";
        }
    }

    Job job;

    public override void _Ready()
    {
        btnMinus.Pressed += () =>
        {
            if (Count <= 0)
                return;

            Game.Raccoons++;
            Count--;
        };

        btnPlus.Pressed += () =>
        {
            if (Game.Raccoons <= 0)
                return;

            Game.Raccoons--;
            Count++;
        };
    }
}
