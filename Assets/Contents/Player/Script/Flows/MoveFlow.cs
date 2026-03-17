using UnityEngine;

public class MoveFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;
        
    PlayerMoveAbility moveAbility;
    Vector2 dir;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        var brain = Processor.Entity as Brain;
        var entity = brain.Controll as Entity;
        moveAbility = entity.GetAbility<PlayerMoveAbility>();
            
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

        if (moveAbility == null)
        {
            return;
        }

        moveAbility.Move(new Vector2(dir.x, dir.y));
    }

    public void SetDuration(float duration)
    {
        Duration = duration;
    }
}