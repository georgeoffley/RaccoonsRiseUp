namespace RRU;

public partial class PopupManager : Node
{
    static List<UIPopup> popupQueue = new();
    static PopupManager instance;

    public override void _Ready()
    {
        instance = this;
    }

    public static void QueuePopup(UIPopup popup)
    {
        // Add this popup to the queue
        popupQueue.Add(popup);

        // This is fired when the popup gets queue freed
        popup.TreeExited += () =>
        {
            // Remove the popup from the queue
            popupQueue.Remove(popup);

            // Try to spawn the next popup
            if (popupQueue.Count > 0)
                AddPopup(popupQueue[0]);
        };

        // Checking if count > 1 instead of > 0 because have to account for
        // the popup that was just added to the queue
        if (popupQueue.Count > 1)
            return;

        // Add the popup to the scene tree
        AddPopup(popup);
    }

    static void AddPopup(UIPopup popup) =>
        instance.GetTree().Root.GetNode("Game").AddChild(popup);
}
