namespace RRU;

public sealed partial class TechNodeDetails : Control
{
	private const string TextStatusLearned = "Learned";
	private const string TextStatusNotLearned = "Not Learned";

	[Export]
	private HSplitContainer _splitView;

	private Tween _tween;

	private TextureRect _icon;
	private Label _labelType;
	private Label _labelDescription;
	private Label _labelStatus;
	private Button _buttonLearn;

	private bool _wasVisible;

    public override void _Ready()
    {
		_icon = GetNode<TextureRect>("%Icon");
		_labelType = GetNode<Label>("%Type");
		_labelDescription = GetNode<Label>("%Description");
		_labelStatus = GetNode<Label>("%LearnState");

		_buttonLearn = GetNode<Button>("%BtnLearn");
		_buttonLearn.Pressed += OnLearnPressed;

		SetLearnState(false);
		Modulate = Colors.Transparent;
    }

	/// Helpers ///

	private void SetLearnState(bool isLearned)
	{
		_buttonLearn.Disabled = isLearned;
		_labelStatus.Text = isLearned ? TextStatusLearned : TextStatusNotLearned;
	}

	private void SetVisibility(bool visible)
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

	public void OnShowDetailRequested(TechNodeInfo info)
	{
		if (info == null)
		{
			OnHideRequested();
			return;
		}

		SetVisibility(true);

		// TODO: Maybe change this into something else later ??
		_labelType.Text = info.Type.ToString();

		_labelDescription.Text = info.Data.Description;
		_icon.Texture = info.Data.GetImage();

		// TODO: Implement 'learn' state
		SetLearnState(false);
	}

	public void OnHideRequested()
	{
		SetVisibility(false);
	}

	private void OnLearnPressed()
	{
		// TODO: Implement
	}
}
