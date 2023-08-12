namespace RRU;

/// <summary>
/// A resource modifier defines how a resource will be affected by a tech 
/// upgrade/structure
/// </summary>
[GlobalClass]
public sealed partial class ResourceModifierDefinition : Resource, IResourceModifier
{
    [Export] public ResourceType TargetResource;

    [Export] public ResourceModifierType ModType;
    [Export] public double ModValue;

    /// Resource Modifier ///

    public bool ModifierIsActive(GameState _, double __) => true;

    public void ModifierGet(GameState _, ref ResourceModifier modifier)
    {
        modifier = new(
            resource: TargetResource,
            type: ModType,
            amount: ModValue
        );
    }
}
