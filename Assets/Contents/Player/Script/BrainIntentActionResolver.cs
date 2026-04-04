public class BrainIntentActionResolver
{
    public bool TryResolve(BrainActionRequest request, BrainActionExecutionContext context)
    {
        switch (request.IntentActionType)
        {
            case IntentActionType.FollowTarget:
                return FollowTargetActionLogic.TryExecute(context);
            case IntentActionType.UseMainAttackSkill:
                return UseSkillActionLogic.TryExecuteByInputAction(context, InputActionType.MainAttack);
            default:
                return false;
        }
    }
}
