namespace RRU;

public partial class UIStructures : Node
{
    GridContainer gridContainer;

    public override void _Ready()
    {
        gridContainer = GetNode<GridContainer>("GridContainer");

        AddStructure("Lumber Camp", 100, TechType.WoodEffeciency);
        AddStructure("Research Camp", 75, TechType.ResearchEffeciency);
    }

    void AddStructure(string name, int cost, TechType techType)
    {
        var structure = Prefabs.Structure.Instantiate<UIStructure>()
            .UpdateName(name)
            .UpdateCost(cost)
            .UpdateIcon(techType);

        gridContainer.AddChild(structure);
    }
}
