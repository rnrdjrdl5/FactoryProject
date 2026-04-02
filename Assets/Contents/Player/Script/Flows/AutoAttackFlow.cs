public class AutoAttackFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    IBrainActionRequester actionRequester;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionRequester = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        actionRequester?.RequestAction(new PerformIntentActionRequest
        {
            IntentActionType = IntentActionType.UseMainAttackSkill
        });

        if (elapsedTime >= Duration)
        {
            Finish();
        }
    }
}
