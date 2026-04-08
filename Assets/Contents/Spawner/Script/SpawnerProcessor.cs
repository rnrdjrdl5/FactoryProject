using UnityEngine;

public class SpawnerProcessor : Processor
{
    MainRealm mainRealm;
    MainRealmProcessor realmProcessor;
    MainRealmTeamProcessor teamProcessor;
    
    Spawner spawner;
    Team spawnTeam;
    
    TimerAbility timerAbility;
    RoundAbility roundAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        mainRealm = Entity.GetParent<MainRealm>();
        var mainRealmProcessorAbility = mainRealm.GetAbility<MainRealmProcessorAbility>();
        realmProcessor = mainRealmProcessorAbility.GetProcessor<MainRealmProcessor>();
        teamProcessor = mainRealmProcessorAbility.GetProcessor<MainRealmTeamProcessor>();
        
        spawner = Entity as Spawner;
        spawnTeam = teamProcessor.CreateTeam(TeamType.Spawner, spawner);

        timerAbility = Entity.GetAbility<TimerAbility>();
        timerAbility.SetTimerInterval(spawner.SpawnerData.tick);
        
        roundAbility = Entity.GetAbility<RoundAbility>();

        timerAbility.OnTimer += OnTimer;
    }

    public override void Uninitialize()
    {
        timerAbility.OnTimer -= OnTimer;
        teamProcessor.TryRemoveTeam(spawnTeam);
        spawnTeam = null;
        
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
        var playerInitData = new PlayerInitData()
        {
            PlayerKey = spawnedPlayerKey,
            Position = position,
            OriginType = PlayerOriginType.WorldSpawned
        };

        var brainAbility = Realm.GetAbility<BrainAbility>();
        if (brainAbility == null)
        {
            return null;
        }

        var tuple = brainAbility.CreateBrainAndControlled<Player>(Brain.PrefabPath, playerData.prefabPath, null, playerInitData);
        var brain = tuple.brain;
        var player = tuple.controlled;
        spawnTeam.TryAddPlayer(player);

        brain.SetControlMode(BrainControlMode.AI);
        
        var brainProcessorAbility = brain.GetAbility<BrainProcessorAbility>();
        var brainFlowProcessor = brainProcessorAbility.GetProcessor<BrainFlowProcessor>();
        brainFlowProcessor.ChangeFlow<EnemyAIFlow>();
        
        return brain;
    }
}
