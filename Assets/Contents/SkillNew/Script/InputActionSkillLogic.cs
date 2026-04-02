public static class InputActionSkillLogic
{
    public static bool TrySetSkillKey(PlayerData playerData, InputActionType inputActionType, string skillKey)
    {
        if (playerData?.InputActionSkillData == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(skillKey))
        {
            playerData.InputActionSkillData.ClearSkillKey(inputActionType);
            return true;
        }

        var skillData = Tables.Skill.Get(skillKey);
        if (skillData == null)
        {
            return false;
        }

        playerData.InputActionSkillData.SetSkillKey(inputActionType, skillKey);
        return true;
    }

    public static bool TryClearSkillKey(PlayerData playerData, InputActionType inputActionType)
    {
        if (playerData?.InputActionSkillData == null)
        {
            return false;
        }

        playerData.InputActionSkillData.ClearSkillKey(inputActionType);
        return true;
    }

    public static bool TryGetSkillKey(PlayerData playerData, InputActionType inputActionType, out string skillKey)
    {
        skillKey = string.Empty;
        if (playerData?.InputActionSkillData == null)
        {
            return false;
        }

        return playerData.InputActionSkillData.TryGetSkillKey(inputActionType, out skillKey);
    }
}
