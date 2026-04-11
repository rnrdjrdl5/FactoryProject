public class IdleFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    BrainActionProcessor actionProcessor;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionProcessor = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
        actionProcessor?.RequestAction(BrainActionRequest.Input(InputActionType.Move));
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
