namespace RRU;

public partial class Game
{
    Dictionary<StructureType, StructureData> structureData = new()
    {
        {
            StructureType.LumberCamp, new StructureData
            {
                Resources = new()
                {
                    {
                        ResourceType.Wood, new ResourceData
                        {
                            GatherRate = 0.1,
                            GatherAmount = 1
                        }
                    }
                }
            }
        },
        {
            StructureType.ResearchCamp, new StructureData
            {
                Resources = new()
                {
                    {
                        ResourceType.Tech, new ResourceData
                        {
                            GatherRate = 0.5,
                            GatherAmount = 1
                        }
                    }
                }
            }
        }
    };
}
