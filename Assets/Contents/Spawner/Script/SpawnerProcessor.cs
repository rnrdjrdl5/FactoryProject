using UnityEngine;

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
        SpawnPlayerAndBrain(roundAbility.GetRandomPoint());
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