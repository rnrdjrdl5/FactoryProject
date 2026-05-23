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
        var mainRealmContext = mainRealm.GetProcessorContext<MainRealmProcessorContext>();
        teamProcessor = mainRealmContext?.MainRealmTeamProcessor;
        playerEntityProcessor = mainRealmContext?.MainRealmPlayerEntityProcessor;
        
        spawner = Entity as Spawner;
        spawnTeam = teamProcessor.CreateTeam(TeamType.Spawner, spawner, spawner.UniqueId);

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
        playerEntityProcessor.CreatePlayerBySpawner(spawner, spawnTeam, roundAbility.GetRandomPoint());
    }
}
