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
        set => labelRaccoons.Text = value + "";
    }

    public int Woodcutters
    {
        get => int.Parse(labelWoodcutters.Text);
        set => labelWoodcutters.Text = value + "";
    }

    public int Researchers
    {
        get => int.Parse(labelResearchers.Text);
        set => labelResearchers.Text = value + "";
    }

    public override void _Ready()
    {
        UIJob.RaccoonAssigned += job => Raccoons--;
        UIJob.RaccoonUnassigned += job => Raccoons++;
    }
}
