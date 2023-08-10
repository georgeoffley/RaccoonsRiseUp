namespace RRU;

public class CommandExit : Command
{
    public CommandExit() => Aliases = new[] { "quit" };

    public override void Run(string[] args)
    {
        Global.QuitAction?.Invoke();
    }
}
