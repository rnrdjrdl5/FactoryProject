public class BrainProcessor : Processor
{
    FlowRunnerAbility flowRunnerAbility;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        flowRunnerAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();
    }
    
    public void SetAIBrain()
    {
        var aiFlow = flowRunnerAbility.Flow.GetFlowAll<BrainFlowProcessor.AIFlow>();
        aiFlow.Parent.ActivateChildFlow<BrainFlowProcessor.AIFlow>();
    }
    
    public void SetPlayerBrain()
    {
        var noneFlow = flowRunnerAbility.Flow.GetFlowAll<BrainFlowProcessor.NoneFlow>();
        noneFlow.Parent.ActivateChildFlow<BrainFlowProcessor.NoneFlow>();
    }
}
