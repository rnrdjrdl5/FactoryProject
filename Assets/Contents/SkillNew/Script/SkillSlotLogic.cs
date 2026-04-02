public static class SkillSlotLogic
{
    public static bool TrySetSkillKey(PlayerData playerData, SkillSlotType slotType, string skillKey)
    {
        if (playerData?.SkillSlotData == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(skillKey))
        {
            playerData.SkillSlotData.ClearSkillKey(slotType);
            return true;
        }

        var skillData = Tables.Skill.Get(skillKey);
        if (skillData == null)
        {
            return false;
        }

        playerData.SkillSlotData.SetSkillKey(slotType, skillKey);
        return true;
    }

    public static bool TryClearSkillKey(PlayerData playerData, SkillSlotType slotType)
    {
        if (playerData?.SkillSlotData == null)
        {
            return false;
        }

        playerData.SkillSlotData.ClearSkillKey(slotType);
        return true;
    }

    public static bool TryGetSkillKey(PlayerData playerData, SkillSlotType slotType, out string skillKey)
    {
        skillKey = string.Empty;
        if (playerData?.SkillSlotData == null)
        {
            return false;
        }

        return playerData.SkillSlotData.TryGetSkillKey(slotType, out skillKey);
    }
}
