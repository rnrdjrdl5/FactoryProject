public class AutoAttackFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    BrainActionProcessor actionProcessor;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionProcessor = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        actionProcessor?.RequestAction(new PerformIntentActionRequest
        {
            IntentActionType = IntentActionType.UseMainAttackSkill
        });

        if (elapsedTime >= Duration)
        {
            Finish();
        }
    }
}
