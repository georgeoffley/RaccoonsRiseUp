namespace RRU;

[GlobalClass]
public sealed partial class JobData: Resource, IResourceModifier
{
    [Export] public JobType Job;
    [Export] public ResourceType ResourceType;

    [Export] double gatherRate;
    [Export] double gatherAmount;

    double elapsedTime;

    /// Resource Modifier ///

    public bool ModifierIsActive(GameState context, double delta)
    {
        if (context.Jobs[Job] == 0)
            return false;

        elapsedTime += delta;

        if (elapsedTime < gatherRate)
            return false;

        elapsedTime = 0.0f;
        return true;
    }

    public void ModifierGet(GameState context, ref ResourceModifier modifier)
    {
        modifier = new(
            ResourceType,
            ResourceModifierType.Additive,
            gatherAmount * context.Jobs[Job]
        );
    }
}

public enum JobType
{
    Woodcutter,
    Researcher
}
