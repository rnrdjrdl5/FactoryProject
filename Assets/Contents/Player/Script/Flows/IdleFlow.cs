public class IdleFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;
        
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