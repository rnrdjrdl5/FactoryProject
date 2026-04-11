public static class FollowTargetActionLogic
{
    public static bool TryExecute(BrainActionExecutionContext context)
    {
        if (context?.FollowAbility == null)
        {
            return false;
        }

        var moveDelta = context.FollowAbility.Move();
        var stateType = moveDelta.sqrMagnitude > 0f ? PixemAnimationType.Run : PixemAnimationType.Idle;
        if (moveDelta.x != 0f)
        {
            context.ModelProcessor.SetFlip(moveDelta.x > 0f);
        }

        context.ModelProcessor.SetStateType(stateType);
        return true;
    }
}
