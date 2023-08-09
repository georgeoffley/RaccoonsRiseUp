namespace RRU;

public partial class UITechNode : Control
{
    private const int DescriptionFontSize = 32;
    private const int DescriptionOffset = 125;

    public static event Action<Vector2> ClickedOnNode;

    [Signal]
    public delegate void ShowDetailRequestEventHandler(TechNodeInfo info);

    public bool IsActive { get; set; }

    GTween tweenScale;
    TechNodeInfo info;

    public override void _Ready()
    {
        PivotOffset += Size / 2;

        MouseEntered += OnHoverEnter;
        MouseExited += OnHoverExit;
        GuiInput += OnGuiInput;
    }

    public void Setup(TechNodeInfo info)
    {
        this.info = info;
        SetImage(this.info.Type);
    }

    void SetImage(TechType techType)
    {
        var imagePath = $"res://Sprites/Icons/{Game.TechData[techType].ImagePath}.svg";
        var textureRect = GetNode<TextureRect>("TextureRect");

        textureRect.Texture = GD.Load<Texture2D>(imagePath);
    }

    void AnimateScale(float scale, int zindex, double duration = 0.1)
    {
        ZIndex = zindex;

        tweenScale = new GTween(this);
        tweenScale.Animate("scale", Vector2.One * scale, duration)
            .SetTrans(Tween.TransitionType.Sine);
    }

    public void Deactivate()
    {
        ZIndex = 0;

        AnimateScale(1,
            zindex: 0,
            duration: 0.2);
    }

    /// Signal Handlers ///

    private void OnHoverEnter()
    {
        if (UITech.TechNodeActive)
            return;

        AnimateScale(1.05f,
            zindex: 100,
            duration: 0.1);
    }

    private void OnHoverExit()
    {
        if (UITech.TechNodeActive)
            return;

        AnimateScale(1,
            zindex: 0,
            duration: 0.1);
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (IsActive ||
            @event is not InputEventMouseButton mouse ||
            UITech.TechNodeActive ||
            !mouse.IsLeftClickPressed())
        {
            return;
        }

        IsActive = true;
        UITech.TechNodeActive = true;

        AnimateScale(
            scale: 2,
            zindex: 100,
            duration: 0.2
        );

        ClickedOnNode?.Invoke(Position + Size / 2);
        EmitSignal(SignalName.ShowDetailRequest, info);

        GetViewport().SetInputAsHandled();
    }
}
