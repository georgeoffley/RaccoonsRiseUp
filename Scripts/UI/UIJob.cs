namespace Template;

public partial class UIJob : Node
{
    [Export] public Label labelName;
    [Export] public Label labelCount;
    [Export] public Button btnMinus;
    [Export] public Button btnPlus;

    public string JobName
    {
        get => labelName.Text;
        set => labelName.Text = value;
    }

    public int Count
    {
        get => int.Parse(labelCount.Text);
        set => labelCount.Text = value + "";
    }

    public override void _Ready()
    {
        btnMinus.Pressed += () =>
        {

        };

        btnPlus.Pressed += () =>
        {

        };
    }
}
