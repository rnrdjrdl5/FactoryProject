public class AutoAttackFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        if (elapsedTime >= Duration)
        {
            Finish();
        }
    }
}
