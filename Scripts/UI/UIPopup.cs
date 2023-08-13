namespace RRU;

public partial class UIPopup : PanelContainer
{
    double duration = 4;
    int dir = 1;

    public override async void _Ready()
    {
        await Animate();
    }

    public UIPopup SetDuration(double duration)
    {
        this.duration = duration;
        return this;
    }

    /// <summary>
    /// Set the icon for the popup.
    /// </summary>
    public UIPopup SetIcon(Texture2D texture)
    {
        GetNode<TextureRect>("%Icon").Texture = texture;
        return this;
    }

    /// <summary>
    /// Set which corner the popup should reside in.
    /// </summary>
    public UIPopup SetLayout(Layout layout)
    {
        switch (layout)
        {
            case Layout.TopLeft:
                SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
                SizeFlagsVertical = SizeFlags.ShrinkBegin;
                dir = -1;
                break;
            case Layout.TopRight:
                SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
                SizeFlagsVertical = SizeFlags.ShrinkBegin;
                dir = -1;
                break;
            case Layout.TopCenter:
                SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
                SizeFlagsVertical = SizeFlags.ShrinkBegin;
                dir = -1;
                break;
            case Layout.BottomLeft:
                SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
                SizeFlagsVertical = SizeFlags.ShrinkEnd;
                dir = 1;
                break;
            case Layout.BottomRight:
                SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
                SizeFlagsVertical = SizeFlags.ShrinkEnd;
                dir = 1;
                break;
            case Layout.BottomCenter:
                SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
                SizeFlagsVertical = SizeFlags.ShrinkEnd;
                dir = 1;
                break;
        }

        return this;
    }

    /// <summary>
    /// Set the description of the popup.
    /// </summary>
    public UIPopup SetDescription(string text)
    {
        GetNode<Label>("%Description").Text = text;
        return this;
    }

    /// <summary>
    /// Set the color of the popup.
    /// </summary>
    public UIPopup SetColor(Color color)
    {
        // Set the border color
        StyleBoxFlat styleBox = GetThemeStylebox("panel") as StyleBoxFlat;
        styleBox.BorderColor = color;
        Set("theme_override_styles/panel", styleBox); // there does not seem to be a better way of doing this right now

        // Set the background gradient color
        PanelContainer innerPanel = GetNode<PanelContainer>("%InnerPanel");

        StyleBoxTexture styleBoxInner = innerPanel.
            GetThemeStylebox("panel") as StyleBoxTexture;

        GradientTexture2D texture = styleBoxInner.Texture as GradientTexture2D;

        Color gradientColor1 = Color.Color8(
            (byte)(color.R8 - 30), 
            (byte)color.G8, 
            (byte)color.B8, 
            (byte)color.A8);

        Color gradientColor2 = Color.Color8(
            (byte)(color.R8 + 92),
            (byte)color.G8,
            (byte)color.B8,
            (byte)color.A8);

        Color[] colors = texture.Gradient.Colors;
        colors[0] = gradientColor1;
        colors[1] = gradientColor2;
        texture.Gradient.Colors = colors;

        return this;
    }

    async Task Animate()
    {
        await GUtils.WaitOneFrame(this);
        Position += new Vector2(0, Size.Y) * dir;

        var tween = new GTween(this);
        AnimateReveal(tween, transDuration: 1);
        AnimateHide(tween, transDuration: 1.5);
        tween.Callback(() => QueueFree());
    }

    void AnimateReveal(GTween tween, double transDuration) =>
        tween.Animate("position", Position - new Vector2(0, Size.Y) * dir, transDuration)
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.Out);

    void AnimateHide(GTween tween, double transDuration) =>
        tween.Animate("position", Position + new Vector2(0, Size.Y) * dir, transDuration)
            .SetDelay(this.duration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

    public enum Layout
    {
        BottomCenter,
        BottomRight,
        BottomLeft,
        TopCenter,
        TopRight,
        TopLeft
    }
}
