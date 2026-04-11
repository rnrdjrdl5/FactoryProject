using UnityEngine;

public static class MoveActionLogic
{
    public static bool TryExecute(BrainActionExecutionContext context, Vector2 direction)
    {
        if (context?.MoveAbility == null)
        {
            return false;
        }

        var moveDelta = context.MoveAbility.Move(direction);
        var stateType = moveDelta.sqrMagnitude > 0f ? PixemAnimationType.Run : PixemAnimationType.Idle;
        if (moveDelta.x != 0f)
        {
            context.ModelProcessor.SetFlip(moveDelta.x > 0f);
        }

        context.ModelProcessor.SetStateType(stateType);
        return true;
    }
}
