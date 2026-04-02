using UnityEngine;

public class MoveFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;

    IBrainActionRequester actionRequester;
    Vector2 dir;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        actionRequester = Processor.ProcessorAbility.GetProcessor<BrainActionProcessor>();
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

        actionRequester?.RequestAction(new PerformCustomActionRequest
        {
            CustomActionType = CustomActionType.Move,
            Direction = new Vector2(dir.x, dir.y)
        });
    }

    public void SetDuration(float duration)
    {
        Duration = duration;
    }
}
