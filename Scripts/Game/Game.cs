namespace RRU;

using Newtonsoft.Json;

public partial class Game : Node
{
    [Export] GameState gameState;

    [Export] TechDataService techData;

    [Export] UIInfo pageInfo;
    [Export] UIJobs pageJobs;

    public override void _EnterTree()
    {
        LoadGame();
    }

    public override void _Ready()
    {
        GetNode<Global>(Global.GetNodePath)
            .OnQuitRequest += SaveGame;
    }

    public void SaveGame()
    {
        var saveData = new SaveData
        {
            Raccoons = gameState.Raccoons,
            NumJobs = gameState.Jobs,
            NumResources = gameState.Resources,
            NumStructures = gameState.Structures,
            ResearchedUpgrades = techData.Serialise()
        };

        var content = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        using var file = FileAccess.Open("user://save_game.dat", FileAccess.ModeFlags.Write);
        file.StoreString(content);
        file.Close();
    }

    public void LoadGame()
    {
        if (!FileAccess.FileExists("user://save_game.dat"))
            return;

        using var file = FileAccess.Open("user://save_game.dat", FileAccess.ModeFlags.Read);
        string content = file.GetAsText();

        var saveData = JsonConvert.DeserializeObject<SaveData>(content);

        gameState.Raccoons = saveData.Raccoons;
        gameState.Jobs = saveData.NumJobs;
        gameState.Resources = saveData.NumResources;
        gameState.Structures = saveData.NumStructures;

        if (saveData.ResearchedUpgrades != null)
        {
            techData.Deserialise(saveData.ResearchedUpgrades);
        }

        gameState.UpdateResources();
        gameState.UpdateJobs();
    }

    public override void _Process(double delta)
    {
        gameState.ProcessResourceTick(delta);
    }
}
