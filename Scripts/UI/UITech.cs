namespace RRU;

public partial class UITech : SubViewport
{
    [Export] int techNodeSpacing;

    public static bool TechNodeActive { get; set; }

    Camera2D camera;
    ColorRect overlay; // transparent overlay
    GTween tweenOverlayColor;
    GTween tweenCamera;

    public override void _Ready()
    {
        camera = GetNode<Camera2D>("Camera2D");

        CreateTransparentOverlay();
        AddTechNodes();

        UITechNode.ClickedOnNode += techNodePos =>
        {
            // Camera moves to the tech node that was clicked
            tweenCamera = new GTween(camera);
            tweenCamera.Animate("position", techNodePos, 0.5)
                .SetTrans(Tween.TransitionType.Sine);

            // Animate overlay to a nice dark transparent background
            tweenOverlayColor = new GTween(overlay);
            tweenOverlayColor.Animate("color", new Color(0, 0, 0, 0.8f), 0.2);
        };
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (!TechNodeActive)
                return;

            if (key.Keycode == Key.Escape)
            {
                UITechNode activeTechNode = FindActiveTechNode();
                DeactivateTechNode(activeTechNode);
            }
        }

        if (@event is InputEventMouseButton mouse)
        {
            if (!TechNodeActive || mouse.IsLeftClickReleased())
                return;

            UITechNode activeTechNode = FindActiveTechNode();

            if (!activeTechNode.GetRect().HasPoint(activeTechNode.GetGlobalMousePosition()))
            {
                // Clicked outside of active tech node
                DeactivateTechNode(activeTechNode);
            }
        }
    }

    List<UITechNode> techNodes = new();

    UITechNode FindActiveTechNode()
    {
        foreach (var techNode in techNodes)
            if (techNode.IsActive)
                return techNode;

        return null;
    }

    void AddTechNodes()
    {
        for (int x = 0; x < 4; x++)
            AddTech(TechType.WoodEffeciency, x, 0);

        AddTech(TechType.ResearchEffeciency, 0, -1);
        AddTech(TechType.ResearchEffeciency, 1, -1);

        foreach (var techNode in techNodes)
            // This needs to be done after all tech nodes are added to the scene
            techNode.CreateDescriptionLabel();
    }

    void CreateTransparentOverlay()
    {
        overlay = new ColorRect();
        overlay.ZIndex = 10;

        // I am using a very large size because changing the size dynamically
        // with the camera zoom was not working out for me
        // Also adding the overlay to a CanvasLayer did not work for me because
        // apparently ZIndex does not hop over to things not in the CanvasLayer
        overlay.Size = DisplayServer.WindowGetSize() * 10;

        overlay.Color = new Color(0, 0, 0, 0);
        overlay.MouseFilter = Control.MouseFilterEnum.Ignore;
        camera.AddChild(overlay);
        overlay.Position -= overlay.Size / 2;
    }

    void DeactivateTechNode(UITechNode activeTechNode)
    {
        activeTechNode.IsActive = false;
        activeTechNode.Deactivate();

        TechNodeActive = false;

        tweenOverlayColor = new GTween(overlay);
        tweenOverlayColor.Animate("color", new Color(0, 0, 0, 0), 0.2);
    }

    void AddTech(TechType techType, int x, int y)
    {
        var techNode = Prefabs.TechNode.Instantiate<UITechNode>();
        techNode.Setup(techType);
        AddChild(techNode);
        techNodes.Add(techNode);

        // Must do this after AddChild(...) otherwise techNode.Size will
        // not be accurate
        var offset = techNode.Size / 2;
        var spacing = Vector2.One * techNodeSpacing;

        techNode.Position = 
            new Vector2(x, y) * (techNode.Size + spacing) - offset;
    }

    /*public override void _Draw()
    {
        DrawLine(new Vector2(-1000, 0), new Vector2(1000, 0), Colors.White, 2);
        DrawLine(new Vector2(0, -1000), new Vector2(0, 1000), Colors.White, 2);
    }*/
}
