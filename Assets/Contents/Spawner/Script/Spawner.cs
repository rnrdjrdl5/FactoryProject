using UnityEngine;

public class Spawner : Entity
{
    public static string PrefabName = "Spawner/Spawner";

    public Tables.Spawner SpawnerData => spawnerData;
    
    Tables.Spawner spawnerData;
    string spawnerKey;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        if (initData is SpawnerInitData spawnerInitData)
        {
            spawnerKey = spawnerInitData.SpawnerKey;
            spawnerData = Tables.Spawner.Get(spawnerKey);
        }
        
        base.Initialize(initData);
    }
}

public class SpawnerInitData : IInitData
{
    public string SpawnerKey;
}
