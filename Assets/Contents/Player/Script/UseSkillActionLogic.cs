public static class UseSkillActionLogic
{
    public static bool TryExecute(BrainActionExecutionContext context, string skillKey)
    {
        if (context?.SkillAbility == null)
        {
            return false;
        }

        return context.SkillAbility.TryUseSkill(skillKey);
    }

    public static bool TryExecuteByInputAction(BrainActionExecutionContext context, InputActionType inputActionType)
    {
        var playerData = context?.ControlledEntity?.GetEntityData<PlayerData>();
        if (!InputActionSkillLogic.TryGetSkillKey(playerData, inputActionType, out var skillKey))
        {
            return false;
        }

        return TryExecute(context, skillKey);
    }
}
