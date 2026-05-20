using UnityEngine;

public static class InputActionSkillLogic
{
    public static bool TrySetSkillKey(PlayerData playerData, KeyCode keyCode, string skillKey)
    {
        if (playerData?.InputActionSkillData == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(skillKey))
        {
            playerData.InputActionSkillData.ClearSkillKey(keyCode);
            return true;
        }

        var skillData = Tables.Skill.Get(skillKey);
        if (skillData == null)
        {
            return false;
        }

        playerData.InputActionSkillData.SetSkillKey(keyCode, skillKey);
        return true;
    }

    public static bool TryClearSkillKey(PlayerData playerData, KeyCode keyCode)
    {
        if (playerData?.InputActionSkillData == null)
        {
            return false;
        }

        playerData.InputActionSkillData.ClearSkillKey(keyCode);
        return true;
    }

    public static bool TryGetSkillKey(PlayerData playerData, KeyCode keyCode, out string skillKey)
    {
        skillKey = string.Empty;
        if (playerData?.InputActionSkillData == null)
        {
            return false;
        }

        return playerData.InputActionSkillData.TryGetSkillKey(keyCode, out skillKey);
    }

    public static bool TryGetMainAttackSkillKey(PlayerData playerData, out string skillKey)
    {
        return TryGetSkillKey(playerData, KeyCode.Mouse0, out skillKey);
    }
}
