using UnityEngine;

public class MoveFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    BrainActionProcessor brainActionProcessor;
    Vector2 dir;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        brainActionProcessor = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
        dir = Random.insideUnitCircle;
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        if (elapsedTime >= Duration)
        {
            parent.ActivateChildFlow<IdleFlow>();
            return;
        }

        brainActionProcessor?.Move(new Vector2(dir.x, dir.y));
    }

    public void SetDuration(float duration)
    {
        Duration = duration;
    }
}
