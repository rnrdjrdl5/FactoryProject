public class SpawnerProcessor : Processor
{
    TimerAbility timerAbility;
    SpawnEntityAbility spawnEntityAbility;
    RoundAbility roundAbility;

    public override void Ready()
    {
        base.Ready();

        timerAbility = Entity.GetAbility<TimerAbility>();
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
        var randomPoint = roundAbility.GetRandomPoint();
        
        spawnEntityAbility.SpawnEntity(randomPoint);
    }
}