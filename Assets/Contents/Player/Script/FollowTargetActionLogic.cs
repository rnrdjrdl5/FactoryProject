public static class FollowTargetActionLogic
{
    public static bool TryExecute(BrainActionExecutionContext context)
    {
        if (context?.FollowAbility == null)
        {
            return false;
        }

        context.FollowAbility.Move();
        return true;
    }
}
