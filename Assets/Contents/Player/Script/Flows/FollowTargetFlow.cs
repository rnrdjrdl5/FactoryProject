public class FollowTargetFlow : ProcessorFlow
{
    IBrainActionRequester actionRequester;
    
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionRequester = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        actionRequester?.RequestAction(new FollowTargetActionRequest());
    }
}
