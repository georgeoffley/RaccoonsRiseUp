namespace RRU;

[GlobalClass]
public sealed partial class ResourceRequirement : Resource
{
    [Export] public ResourceType Type;
    [Export] public int Amount;
}
