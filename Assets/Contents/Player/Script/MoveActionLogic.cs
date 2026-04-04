using UnityEngine;

public static class MoveActionLogic
{
    public static bool TryExecute(BrainActionExecutionContext context, Vector2 direction)
    {
        if (context?.MoveAbility == null)
        {
            return false;
        }

        context.MoveAbility.Move(direction);
        return true;
    }
}
