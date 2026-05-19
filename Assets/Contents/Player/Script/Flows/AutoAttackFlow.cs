public class AutoAttackFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    BrainActionProcessor actionProcessor;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionProcessor = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
        actionProcessor?.RequestAction(new MoveBrainAction());
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        actionProcessor?.RequestAction(new UseMainAttackSkillBrainAction());

        if (elapsedTime >= Duration)
        {
            Finish();
        }
    }
}
