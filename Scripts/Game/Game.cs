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

    public override void _PhysicsProcess(double delta)
    {
        var resourcesChanged = false;

        ResourcesGainedByStructures(delta, ref resourcesChanged);
        ResourcesGainedByJobs(delta, ref resourcesChanged);

        if (resourcesChanged)
            gameState.UpdateResources();
    }

    void ResourcesGainedByStructures(double delta, ref bool resourcesChanged)
    {
        foreach (var structure in structureData)
        {
            var structureData = structure.Value;

            if (gameState.Structures[structure.Key] == 0)
                continue;

            structureData.Resources.ForEach(x => x.Value.ElpasedTime += delta);

            foreach (var resource in structureData.Resources)
            {
                var resourceData = resource.Value;
                if (resourceData.ElpasedTime >= resourceData.GatherRate)
                {
                    var timesEarned = resourceData.ElpasedTime / resourceData.GatherRate;

                    resourceData.ElpasedTime -= resourceData.GatherRate * timesEarned;

                    gameState.Resources[resource.Key] +=
                        resourceData.GatherAmount * timesEarned * gameState.Structures[structure.Key];

                    resourcesChanged = true;
                }
            }
        }
    }

    void ResourcesGainedByJobs(double delta, ref bool resourcesChanged)
    {
        foreach (var job in jobData)
        {
            var jobData = job.Value;

            if (gameState.Jobs[job.Key] == 0)
                continue;

            jobData.ElpasedTime += delta;

            if (jobData.ElpasedTime >= jobData.GatherRate)
            {
                var timesEarned = jobData.ElpasedTime / jobData.GatherRate;

                jobData.ElpasedTime -= jobData.GatherRate * timesEarned;

                gameState.Resources[jobData.ResourceType] +=
                    jobData.GatherAmount * timesEarned * gameState.Jobs[job.Key];

                resourcesChanged = true;
            }
        }
    }
}
