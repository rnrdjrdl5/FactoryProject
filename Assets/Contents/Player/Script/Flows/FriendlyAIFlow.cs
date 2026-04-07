public class FriendlyAIFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();
        
        var followFlow = AddChild<FollowTargetFlow>(Processor);
        followFlow.AddChild<AutoAttackFlow>(Processor);
        followFlow.SetLoop(true);
    }
}

