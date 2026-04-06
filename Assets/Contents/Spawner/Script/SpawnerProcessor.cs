using UnityEngine;

public class SpawnerProcessor : Processor
{
    MainRealm mainRealm;
    MainRealmProcessor realmProcessor;
    
    Spawner spawner;
    
    TimerAbility timerAbility;
    RoundAbility roundAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        mainRealm = Entity.GetParent<MainRealm>();
        var mainRealmProcessorAbility = mainRealm.GetAbility<MainRealmProcessorAbility>();
        realmProcessor = mainRealmProcessorAbility.GetProcessor<MainRealmProcessor>();
        
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

        var brainAbility = Realm.GetAbility<BrainAbility>();
        if (brainAbility == null)
        {
            return null;
        }

        var tuple = brainAbility.CreateBrainAndControlled<Player>(Brain.PrefabPath, playerData.prefabPath, null, playerInitData);
        var brain = tuple.brain;
        brain.SetControlMode(BrainControlMode.AI);
        
        var brainProcessorAbility = brain.GetAbility<BrainProcessorAbility>();
        var brainFlowProcessor = brainProcessorAbility.GetProcessor<BrainFlowProcessor>();
        brainFlowProcessor.ChangeFlow<CommonWildPlayerFlow>();
        
        return brain;
    }
}
