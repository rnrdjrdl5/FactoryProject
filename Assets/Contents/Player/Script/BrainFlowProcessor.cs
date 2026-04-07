using UnityEngine;

public class BrainFlowProcessor : Processor
{
    FlowRunnerAbility flowRunnerAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        flowRunnerAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();
        ChangeFlow<WanderFlow>();
    }

    public void ChangeFlow<TFlow>() where TFlow : ProcessorFlow, new()
    {
        flowRunnerAbility.SetRootProcessorFlow<TFlow>(this);
    }
}
