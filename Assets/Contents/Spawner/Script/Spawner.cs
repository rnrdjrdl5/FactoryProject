using UnityEngine;

public class Spawner : Entity
{
    public static string PrefabName = "Spawner/Spawner";

    public Tables.Spawner SpawnerData => spawnerData;
    public string SpawnerKey { get; private set; }
    
    Tables.Spawner spawnerData;

    public override void Initialize(Parameter parameter)
    {
        SpawnerKey = parameter.Get<string>(nameof(SpawnerKey));
        spawnerData = Tables.Spawner.Get(SpawnerKey);
        
        base.Initialize(parameter);
    }
}
