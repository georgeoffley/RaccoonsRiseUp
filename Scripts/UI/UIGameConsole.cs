namespace RRU;

// Note that there appears to be a bug with Godot when you click the scroll
// bar grab button, it will always be in a 'pressed' state. This is a minor
// annoyance.
public partial class UIGameConsole : PanelContainer
{
    RichTextLabel output;
    double prevScrollValue;

    public override void _Ready()
    {
        output = GetNode<RichTextLabel>("%Output");

        // Auto set the ScrollFollowing property when grabbing the
        // scroll button
        UpdateScrollFollowMouseGrab();
    }

    /// <summary>
    /// Add a message to the game console
    /// </summary>
    public void AddText(object obj)
    {
        var message = $"{obj}\n";
        output.Text += message;

        // The output is getting too long, lets chop it down
        if (output.Text.Length > 1000)
        {
            // Chop down the message based on the length of the message
            // just sent
            output.Text = output.Text.Substring(
                startIndex: message.Length, 
                length: output.Text.Length - message.Length);
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Auto set the ScrollFollowing property when scrolling with the
        // mouse wheel
        UpdateScrollFollowMouseWheel(@event);
    }

    void UpdateScrollFollowMouseWheel(InputEvent @event)
    {
        if (@event is not InputEventMouseButton mBtn)
            return;

        VScrollBar scrollBar = output.GetVScrollBar();

        if (mBtn.IsWheelUp())
        {
            output.ScrollFollowing = false;
        }
        else if (mBtn.IsWheelDown() && CloseToBottomOfScroll(scrollBar))
        {
            output.ScrollFollowing = true;
        }
    }

    void UpdateScrollFollowMouseGrab()
    {
        VScrollBar scrollBar = output.GetVScrollBar();
        scrollBar.Scrolling += () =>
        {
            double diff = scrollBar.Value - prevScrollValue;

            if (diff < 0)
            {
                output.ScrollFollowing = false;
            }
            else
            {
                if (CloseToBottomOfScroll(scrollBar))
                {
                    output.ScrollFollowing = true;
                }
            }

            prevScrollValue = scrollBar.Value;
        };
    }

    bool CloseToBottomOfScroll(VScrollBar scrollBar) =>
        scrollBar.MaxValue - scrollBar.Value <= Size.Y;
}
