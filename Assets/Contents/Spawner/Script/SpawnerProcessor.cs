using UnityEngine;

public class SpawnerProcessor : Processor
{
    Spawner spawner;
    
    TimerAbility timerAbility;
    SpawnEntityAbility spawnEntityAbility;
    RoundAbility roundAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        spawner = Entity as Spawner;
        
        timerAbility = Entity.GetAbility<TimerAbility>();
        timerAbility.SetTimerInterval(spawner.SpawnerData.tick);
        
        spawnEntityAbility = Entity.GetAbility<SpawnEntityAbility>();
        roundAbility = Entity.GetAbility<RoundAbility>();

        timerAbility.OnTimer += OnTimer;
    }

    public override void Uninitialize()
    {
        timerAbility.OnTimer -= OnTimer;
        
        base.Uninitialize();
    }

    void OnTimer()
    {
        SetSpawnPlayerKey();
        SpawnPlayerAndBrain(roundAbility.GetRandomPoint());
    }

    void SetSpawnPlayerKey()
    {
        var spawnedPlayerKey = spawner.SpawnerData.GetSpawnPlayerKey();
        var playerData = Tables.Player.Get(spawnedPlayerKey);
        var prefabPath = playerData.prefabPath;
        
        spawnEntityAbility.SetPrefabPath(prefabPath);
    }
    
    Brain SpawnPlayerAndBrain(Vector3 position)
    {
        var player = spawnEntityAbility.SpawnEntity(position);
        var brain = Realm.AddEntity<Brain>(Brain.PrefabPath);
        brain.AttachControll(player);
        
        SetAIBrain(brain);
        
        return brain;
    }

    void SetAIBrain(Brain brain)
    {
        var processorAbility = brain.GetAbility<BrainProcessorAbility>();
        var brainProcessor = processorAbility.GetProcessor<BrainProcessor>();
        brainProcessor.SetAIBrain();
    }
}
