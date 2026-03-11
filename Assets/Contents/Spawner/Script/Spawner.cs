using UnityEngine;

public class Spawner : Entity
{
    public static string PrefabName = "Spawner/Spawner";

    public Tables.Spawner SpawnerData => spawnerData;
    public string SpawnerKey { get; private set; }
    
    Tables.Spawner spawnerData;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        if (initData is not SpawnerInitData spawnerInitData)
        {
            return;
        }

        SpawnerKey = spawnerInitData.SpawnerKey;
        spawnerData = Tables.Spawner.Get(SpawnerKey);
        
        base.Initialize(initData);
    }
}

public class SpawnerInitData : IInitData
{
    public string SpawnerKey;
}
