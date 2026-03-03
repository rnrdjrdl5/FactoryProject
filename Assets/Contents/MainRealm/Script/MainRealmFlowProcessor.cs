using UnityEngine;

public class MainRealmFlowProcessor : Processor
{
    public override void Initialize()
    {
        base.Initialize();
        
        var flowAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();

        var testFlow = Flow.Create<MainRealmFlow>(ProcessorAbility.Entity);
        testFlow.SetProcessor(this);
        flowAbility.SetRootFlow(testFlow);
        
        testFlow.NextChildFlow();
    }
}

class MainRealmFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<LoadFlow>(Processor);
        AddChild<IngameFlow>(Processor);
    }
}

class LoadFlow : ProcessorFlow
{
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();
        
        Finish();
    }
}

class IngameFlow : ProcessorFlow
{
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        BrainLogic.CreateBrainAndEntity(Processor.Realm, "Player/Brain", "Player/Entity");
        Processor.Realm.AddEntity<SpawnerEntity>("Spawner/SpawnerEntity");
    }
}
