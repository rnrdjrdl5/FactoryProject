public class FollowTargetFlow : ProcessorFlow
{
    BrainActionProcessor actionProcessor;
    
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionProcessor = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        actionProcessor?.RequestAction(new FollowTargetBrainAction());
    }
}
