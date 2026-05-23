public class FollowTargetFlow : ProcessorFlow
{
    BrainActionProcessor actionProcessor;
    
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionProcessor = Processor
            .ProcessorAbility
            .GetContext<BrainProcessorContext>()
            ?.BrainActionProcessor;
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        actionProcessor?.RequestAction(new FollowTargetBrainAction());
    }
}
