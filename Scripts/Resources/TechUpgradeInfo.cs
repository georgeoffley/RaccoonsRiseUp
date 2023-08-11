namespace RRU;

/// <summary>
/// This resource represents an upgrade option in the tech page
/// </summary>
[GlobalClass]
public sealed partial class TechUpgradeInfo : Resource
{
    [Export] public StringName Id;
    [Export] public string[] RequiredUpgradeIds;
    [Export] public ResourceRequirement[] UpgradeCost;
    [Export] public string DisplayName;
    [Export] public TechType UpgradeType;
    [Export] public float Modifier;
    [Export] public Vector2I Position;
}
