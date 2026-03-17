using UnityEngine;

public class BrainFlowProcessor : Processor
{
    FlowRunnerAbility flowRunnerAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        flowRunnerAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();
        ChangeFlow<CommonWildPlayerFlow>();
    }

    public void ChangeFlow<TFlow>() where TFlow : ProcessorFlow, new()
    {
        var rootFlow = Flow.Create<TFlow>(Entity);
        rootFlow.SetProcessor(this);
        flowRunnerAbility.SetRootFlow(rootFlow);
        rootFlow.NextChildFlow();
    }
}
