public class FollowTargetFlow : ProcessorFlow
{
    BrainActionProcessor brainActionProcessor;
    
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        brainActionProcessor = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        brainActionProcessor?.FollowTarget();
    }
}
