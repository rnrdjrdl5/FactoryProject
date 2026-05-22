public class IdleFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    BrainActionProcessor actionProcessor;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionProcessor = Processor
            .ProcessorAbility
            .GetContext<BrainProcessorContext>()
            ?.BrainActionProcessor;
        actionProcessor?.RequestAction(new MoveBrainAction());
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        if (elapsedTime >= Duration)
        {
            parent.ActivateChildFlow<MoveFlow>();
        }
    }

    public void SetDuration(float duration)
    {
        Duration = duration;
    }
}
