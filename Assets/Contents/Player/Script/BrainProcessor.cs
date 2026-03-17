public class BrainProcessor : Processor
{
    FlowRunnerAbility flowRunnerAbility;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        flowRunnerAbility = Entity.GetAbility<FlowRunnerAbility>();
    }
}
