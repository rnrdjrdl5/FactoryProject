using System.Collections.Generic;
using UnityEngine;

public static class BuffEffectParamParser
{ 
    public static IBuffEffectParam Parse(Tables.BuffEffectType type, IReadOnlyList<string> rawParams, string buffKey)
    {
        switch (type)
        {
            case Tables.BuffEffectType.StatModifier:
            {
                var param = new BuffEffectStatModifierParam();
                if (!SkillParamParseUtility.TryGetOptionalEnum<Tables.StatType>(rawParams, 0, buffKey, "statType", out var statType))
                {
                    return null;
                }

                if (!SkillParamParseUtility.TryGetOptionalInt(rawParams, 1, buffKey, "value", out var value))
                {
                    return null;
                }

                param.StatType = statType;
                param.Value = value;
                return param;
            }

            case Tables.BuffEffectType.GrantSkill:
            {
                var param = new BuffEffectGrantSkillParam();
                if (!SkillParamParseUtility.TryGetOptionalString(rawParams, 0, out var skillKey))
                {
                    return null;
                }

                param.SkillKey = skillKey;
                return param;
            }

            case Tables.BuffEffectType.UseSkillOnEnd:
            {
                var param = new BuffEffectUseSkillOnEndParam();
                if (!SkillParamParseUtility.TryGetOptionalString(rawParams, 0, out var skillKey))
                {
                    return null;
                }

                param.SkillKey = skillKey;
                return param;
            }

            default:
                Debug.LogError($"[BuffEffectParamParser] Unsupported BuffEffectType. buffKey={buffKey}, type={type}");
                return null;
        }
    }
}
