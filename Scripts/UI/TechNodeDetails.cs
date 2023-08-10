namespace RRU;

public sealed partial class TechNodeDetails : Control
{
    [Export] TechDataService dataService;
    [Export] HSplitContainer splitView;

    Tween tween;

    TextureRect icon;
    Label labelType;
    Label labelDescription;
    Label labelStatus;
    Button buttonLearn;

    Control prerequisiteView;
    Label prerequisiteLabel;
    Control requirementsView;

    TechInfo info;
    bool wasVisible;

    public override void _Ready()
    {
        prerequisiteLabel = GetNode<Label>("%PrerequisiteLabel");
        prerequisiteView = GetNode<Control>("%Prerequisites");
        requirementsView = GetNode<Control>("%Requirements");
        labelDescription = GetNode<Label>("%Description");
        labelStatus = GetNode<Label>("%LearnState");
        icon = GetNode<TextureRect>("%Icon");
        labelType = GetNode<Label>("%Type");

        buttonLearn = GetNode<Button>("%BtnLearn");
        buttonLearn.Pressed += OnResearchPressed;

        SetLearnState(false);
        Modulate = Colors.Transparent;
    }

    /// Helpers ///

    void SetLearnState(bool isLearned)
    {
        bool isLocked = !dataService.IsUnlocked(info?.Id);

        buttonLearn.Disabled = isLearned || isLocked;
        labelStatus.Text = isLearned ? Tr("RESEARCHED") : Tr("NOT_RESEARCHED");
    }

    void UpdateDetails()
    {
        TechUpgradeInfo upgradeInfo = dataService.GetInfoForId(info.Id);
        ReadOnlySpan<string> requirements = upgradeInfo.RequiredUpgradeIds;

        prerequisiteView.Visible = requirements.Length > 0;

        if (requirements.Length < 1)
            return;

        // Clear requirements
        // Iterate from count to zero
        for (int i = requirementsView.GetChildCount(); i --> 0;)
        {
            requirementsView
                .GetChild(i)
                .QueueFree();
        }

        prerequisiteLabel.Text = requirements.Length > 1 ? 
            "Required Upgrades" : "Required Upgrade";

        // Update requirements
        for (int i = 0; i < requirements.Length; ++ i)
        {
            TechUpgradeInfo requirementInfo = 
                dataService.GetInfoForId(requirements[i]);

            requirementsView.AddChild(
                new GLabel($"* {requirementInfo.DisplayName}"));
        }
    }

    void SetVisibility(bool visible)
    {
        // Prevent the tween from repeating the same operation
        if (wasVisible == visible)
            return;

        // Stop the currently-running tween (if there is any)
        if (IsInstanceValid(tween) && tween.IsRunning())
        {
            tween.Kill();
        }

        tween = CreateTween();
        tween.SetParallel(true);
        wasVisible = visible;

        // Slide
        tween.TweenProperty(
            @object: splitView,
            property: SplitContainer.PropertyName.SplitOffset.ToString(),
            finalVal: visible ? -300 : 0,
            duration: 0.25f
        );

        // Fade
        Modulate = visible ? Colors.Transparent : Colors.White;

        tween.TweenProperty(
            @object: this,
            property: CanvasItem.PropertyName.Modulate.ToString(),
            finalVal: visible ? Colors.White : Colors.Transparent,
            duration: visible ? 0.8f : 0.15f
        );
    }

    /// Signal Handlers ///

    public void OnShowDetailRequested(TechInfo info)
    {
        if (info == null)
        {
            OnHideRequested();
            return;
        }

        TechUpgradeInfo upgradeInfo = dataService.GetInfoForId(id: info.Id);
        this.info = info;

        SetVisibility(true);

        labelType.Text = upgradeInfo.DisplayName;

        labelDescription.Text = info.Data.Description;
        icon.Texture = info.Data.GetImage();

        SetLearnState(dataService.IsLearned(info.Id));
        UpdateDetails();
    }

    public void OnHideRequested()
    {
        SetVisibility(false);
    }

    void OnResearchPressed()
    {
        dataService.Learn(info.Id);
        SetLearnState(true);
    }
}
