public class BrainProcessor : Processor
{
    FlowRunnerAbility flowRunnerAbility;
    
    public override void Ready()
    {
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