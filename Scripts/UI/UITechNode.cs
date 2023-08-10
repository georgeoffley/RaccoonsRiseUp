namespace RRU;

public partial class UITechNode : Control
{
    public static event Action<Vector2> ClickedOnNode;

    static Color LEARNED_COLOUR => new(0.3f, 1.0f, 0.3f, 0.5f);
    static Color LOCKED_COLOUR => new(1.0f, 0.3f, 0.3f, 0.35f);

    const int DESCRIPTION_FONT_SIZE = 32;
    const int DESCRIPTION_OFFSET = 125;

    [Signal]
    public delegate void ShowDetailRequestEventHandler(TechInfo info);

    public bool IsActive { get; set; }

    TextureRect textureRect;
    TechNodeState nodeState;
    GTween tweenScale;
    TechInfo info;

    public override void _Ready()
    {
        nodeState = TechNodeState.Locked;
        textureRect = GetNode<TextureRect>("%TextureRect");
        PivotOffset += Size / 2;

        MouseEntered += OnHoverEnter;
        MouseExited += OnHoverExit;
        GuiInput += OnGuiInput;
    }

    public void Setup(TechInfo info)
    {
        this.info = info;
        textureRect.Texture = info.Data.GetImage();
    }

    public void SetLearnState(TechNodeState state)
    {
        nodeState = state;

        switch (state)
        {
            case TechNodeState.Locked:
                textureRect.Modulate = LOCKED_COLOUR;
                break;

            case TechNodeState.Unlocked:
                textureRect.Modulate = Colors.White;
                break;

            case TechNodeState.Learned:
                textureRect.Modulate = LEARNED_COLOUR;
                break;
        }
    }

    public void Deactivate()
    {
        ZIndex = 0;

        AnimateScale(1,
            zindex: 0,
            duration: 0.2);
    }

    void AnimateScale(float scale, int zindex, double duration = 0.1)
    {
        ZIndex = zindex;

        tweenScale = new GTween(this);
        tweenScale.Animate("scale", Vector2.One * scale, duration)
            .SetTrans(Tween.TransitionType.Sine);
    }

    /// Signal Handlers ///

    public void OnLearnStateChanged(TechDataService service, StringName id, bool isLearned)
    {
        if (nodeState == TechNodeState.Learned)
            return;

        bool isUnlocked = service.IsUnlocked(info.Id);
        SetLearnState(isUnlocked ? TechNodeState.Unlocked : TechNodeState.Locked);

        if (info.Id != id || !isLearned)
            return;

        SetLearnState(TechNodeState.Learned);
    }

    void OnHoverEnter()
    {
        if (UITech.TechNodeActive)
            return;

        AnimateScale(1.05f,
            zindex: 100,
            duration: 0.1);
    }

    void OnHoverExit()
    {
        if (UITech.TechNodeActive)
            return;

        AnimateScale(1,
            zindex: 0,
            duration: 0.1);
    }

    void OnGuiInput(InputEvent @event)
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
            scale: 1.5f,
            zindex: 100,
            duration: 0.2
        );

        ClickedOnNode?.Invoke(Position + Size / 2);
        EmitSignal(SignalName.ShowDetailRequest, info);

        GetViewport().SetInputAsHandled();
    }
}

public enum TechNodeState
{
    Locked,
    Unlocked,
    Learned
}