namespace RRU;

public sealed partial class TechNodeDetails : Control
{
	const string TextStatusLearned = "Learned";
	const string TextStatusNotLearned = "Not Learned";

	const string TextRequiredSingular = "Required Upgrade";
	const string TextRequiredPlural = "Required Upgrades";

	[Export]
	TechDataService _dataService;

	[Export]
	HSplitContainer _splitView;

	Tween _tween;

	TextureRect _icon;
	Label _labelType;
	Label _labelDescription;
	Label _labelStatus;
	Button _buttonLearn;

	Control _prerequisiteView;
	Label _prerequisiteLabel;
	Control _requirementsView;

	TechInfo _info;
	bool _wasVisible;

    public override void _Ready()
    {
		_prerequisiteLabel = GetNode<Label>("%PrerequisiteLabel");
		_prerequisiteView = GetNode<Control>("%Prerequisites");
		_requirementsView = GetNode<Control>("%Requirements");
		_labelDescription = GetNode<Label>("%Description");
		_labelStatus = GetNode<Label>("%LearnState");
		_icon = GetNode<TextureRect>("%Icon");
		_labelType = GetNode<Label>("%Type");

		_buttonLearn = GetNode<Button>("%BtnLearn");
		_buttonLearn.Pressed += OnLearnPressed;

		SetLearnState(false);
		Modulate = Colors.Transparent;
    }

	/// Helpers ///

	void SetLearnState(bool isLearned)
	{
		bool isLocked = !_dataService.IsUnlocked(_info?.Id);

		_buttonLearn.Disabled = isLearned || isLocked;
		_labelStatus.Text = isLearned ? TextStatusLearned : TextStatusNotLearned;
	}

	void UpdateDetails()
	{
		TechUpgradeInfo upgradeInfo = _dataService.GetInfoForId(_info.Id);
		ReadOnlySpan<string> requirements = upgradeInfo.RequiredUpgradeIds;

		_prerequisiteView.Visible = requirements.Length > 0;

		if (requirements.Length < 1)
			return;

		// Clear requirements
		for (int i = _requirementsView.GetChildCount(); i --> 0;)
		{
			_requirementsView
				.GetChild(i)
				.QueueFree();
		}

		_prerequisiteLabel.Text = requirements.Length > 1 ? TextRequiredPlural : TextRequiredSingular;

		// Update requirements
		for (int i = 0; i < requirements.Length; ++ i)
		{
			TechUpgradeInfo requirementInfo = _dataService.GetInfoForId(requirements[i]);

			_requirementsView.AddChild(
				new Label()
				{
					Text = $"* {requirementInfo.DisplayName}"
				}
			);
		}
	}

	void SetVisibility(bool visible)
	{
		// Prevent the tween from repeating the same operation
		if (_wasVisible == visible)
			return;

		// Stop the currently-running tween (if there is any)
		if (IsInstanceValid(_tween) && _tween.IsRunning())
		{
			_tween.Kill();
		}

		_tween = CreateTween();
		_tween.SetParallel(true);
		_wasVisible = visible;

		// Slide
		_tween.TweenProperty(
			@object: _splitView,
			property: SplitContainer.PropertyName.SplitOffset.ToString(),
			finalVal: visible ? -300 : 0,
			duration: 0.25f
		);

		// Fade
		Modulate = visible ? Colors.Transparent : Colors.White;

		_tween.TweenProperty(
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

		TechUpgradeInfo upgradeInfo = _dataService.GetInfoForId(id: info.Id);
		_info = info;

		SetVisibility(true);

		_labelType.Text = upgradeInfo.DisplayName;

		_labelDescription.Text = info.Data.Description;
		_icon.Texture = info.Data.GetImage();

		SetLearnState(_dataService.IsLearned(info.Id));
		UpdateDetails();
	}

	public void OnHideRequested()
	{
		SetVisibility(false);
	}

	void OnLearnPressed()
	{
		_dataService.Learn(_info.Id);
		SetLearnState(true);
	}
}
