using System.Collections.Generic;

public class ResourceManager : Singleton<ResourceManager>
{
    public List<TerrainType> TerrainTypes { get; private set; } = new();
}
