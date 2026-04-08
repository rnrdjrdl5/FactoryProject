public class SpawnerProcessor : Processor
{
    MainRealmTeamProcessor teamProcessor;
    MainRealmPlayerEntityProcessor playerEntityProcessor;
    
    Spawner spawner;
    Team spawnTeam;
    
    TimerAbility timerAbility;
    RoundAbility roundAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        var mainRealm = Entity.GetParent<MainRealm>();
        var mainRealmProcessorAbility = mainRealm.GetAbility<MainRealmProcessorAbility>();
        teamProcessor = mainRealmProcessorAbility.GetProcessor<MainRealmTeamProcessor>();
        playerEntityProcessor = mainRealmProcessorAbility.GetProcessor<MainRealmPlayerEntityProcessor>();
        
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
        playerEntityProcessor.CreateWorldSpawnedPlayer(spawner, spawnTeam, roundAbility.GetRandomPoint());
    }
}
