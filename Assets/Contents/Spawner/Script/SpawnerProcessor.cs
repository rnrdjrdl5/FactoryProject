using UnityEngine;

public class SpawnerProcessor : Processor
{
    Spawner spawner;
    
    TimerAbility timerAbility;
    RoundAbility roundAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        spawner = Entity as Spawner;
        
        timerAbility = Entity.GetAbility<TimerAbility>();
        timerAbility.SetTimerInterval(spawner.SpawnerData.tick);
        
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
        SpawnPlayerAndBrain(roundAbility.GetRandomPoint());
    }
    
    Brain SpawnPlayerAndBrain(Vector3 position)
    {
        var spawnedPlayerKey = spawner.SpawnerData.GetSpawnPlayerKey();
        var playerData = Tables.Player.Get(spawnedPlayerKey);
        var playerInitData = new PlayerInitData() { PlayerKey = spawnedPlayerKey, Position = position };

        var brain = BrainLogic.CreateBrainAndEntity(Realm, Brain.PrefabPath, playerData.prefabPath, null, playerInitData);
        SetAIBrain(brain);
        
        return brain;
    }

    void SetAIBrain(Brain brain)
    {
        brain.SetAI(true);
        
        var processorAbility = brain.GetAbility<BrainProcessorAbility>();
        var brainProcessor = processorAbility.GetProcessor<BrainProcessor>();
        brainProcessor.SetAIBrain();
    }
}
