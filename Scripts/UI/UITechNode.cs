namespace RRU;

public partial class UITechNode : Control
{
    public static event Action<Vector2> ClickedOnNode;

    public bool IsActive { get; set; }

    TechType techType;
    GTween tweenScale;
    GTween tweenLabelColor;
    Label label;

    public override void _Ready()
    {
        PivotOffset += Size / 2;

        MouseEntered += () =>
        {
            if (UITech.TechNodeActive)
                return;

            AnimateScale(1.05f, 
                zindex: 100, 
                duration: 0.1);
        };

        MouseExited += () =>
        {
            if (UITech.TechNodeActive)
                return;

            AnimateScale(1, 
                zindex: 0, 
                duration: 0.1);
        };

        GuiInput += input =>
        {
            if (input is InputEventMouseButton mouse)
            {
                if (!UITech.TechNodeActive)
                {
                    if (mouse.IsLeftClickPressed())
                    {
                        IsActive = true;
                        UITech.TechNodeActive = true;

                        AnimateScale(2,
                            zindex: 100,
                            duration: 0.2);

                        ClickedOnNode?.Invoke(Position + Size / 2);

                        tweenLabelColor = new GTween(label);
                        tweenLabelColor.AnimateColor(new Color(1, 1, 1, 1), 0.3, true);
                        label.Show();
                    }
                }
            }
        };
    }

    public void CreateDescriptionLabel()
    {
        var fontSize = 32;
        var techDesc = Game.TechData[techType].Description;
        label = new GLabel(techDesc, fontSize);
        label.Modulate = new Color(1, 1, 1, 0);
        label.ZIndex = 100;
        GetParent().AddChild(label);

        var offset = 125;
        var pos = Position;
        pos.X += Size.X + offset;
        pos.Y += Size.Y / 2 - fontSize;
        label.Position = pos;
        label.Hide();
    }

    public void Deactivate()
    {
        ZIndex = 0;
        label.Hide();
        tweenLabelColor?.Kill();
        label.Modulate = new Color(1, 1, 1, 0);

        AnimateScale(1,
            zindex: 0,
            duration: 0.2);
    }

    public void Setup(TechType techType)
    {
        this.techType = techType;
        SetImage(techType);
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
}
