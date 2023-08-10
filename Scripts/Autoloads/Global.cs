global using Godot;
global using GodotUtils;
global using System;
global using System.Collections.Generic;
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Linq;
global using System.Runtime.CompilerServices;
global using System.Threading;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;

namespace RRU;

public partial class Global : Node
{
	internal static Action QuitAction;

	public static NodePath GetNodePath
		=> "/root/Global";

	[Signal]
	public delegate void OnQuitRequestEventHandler();

	public override void _Ready()
	{
		GetTree().AutoAcceptQuit = false;

		// For 'CommandExit' to work
		QuitAction = Quit;

        // Gradually fade out all SFX whenever the scene is changed
        SceneManager.SceneChanged += name => AudioManager.FadeOutSFX();
    }

	public override void _PhysicsProcess(double delta)
	{
		Logger.Update();
	}

    public override void _Notification(int what)
	{
		if (what != NotificationWMCloseRequest)
			return;

		CallDeferred(MethodName.Quit);
	}

	public void Quit()
	{
        // Handle cleanup here
        OptionsManager.SaveOptions();
        OptionsManager.SaveHotkeys();

		EmitSignal(SignalName.OnQuitRequest);
        GetTree().CallDeferred(SceneTree.MethodName.Quit);
	}
}
