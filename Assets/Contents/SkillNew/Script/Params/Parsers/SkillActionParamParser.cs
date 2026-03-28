using System.Collections.Generic;
using UnityEngine;

public static class SkillActionParamParser
{
    public static ISkillActionParam Parse(Tables.SkillActionType type, IReadOnlyList<string> rawParams, string skillKey)
    {
        switch (type)
        {
            case Tables.SkillActionType.Damage:
            {
                var param = new SkillActionDamageParam();
                if (!SkillParamParseUtility.TryGetOptionalFloat(rawParams, 0, skillKey, "amount", out var amount))
                {
                    return null;
                }

                param.Amount = amount;
                return param;
            }

            case Tables.SkillActionType.Projectile:
            {
                var param = new SkillActionProjectileParam();
                if (!SkillParamParseUtility.TryGetOptionalFloat(rawParams, 0, skillKey, "speed", out var speed))
                {
                    return null;
                }

                if (!SkillParamParseUtility.TryGetOptionalFloat(rawParams, 1, skillKey, "duration", out var duration))
                {
                    return null;
                }
                
                if (!SkillParamParseUtility.TryGetOptionalString(rawParams, 2, out var prefabPath))
                {
                    return null;
                }

                param.PrefabPath = prefabPath;
                param.Speed = speed;
                param.Duration = duration;
                return param;
            }

            case Tables.SkillActionType.Casting:
            {
                var param = new SkillActionCastingParam();
                if (!SkillParamParseUtility.TryGetOptionalFloat(rawParams, 0, skillKey, "castingTime", out var castingTime))
                {
                    return null;
                }

                param.CastingTime = castingTime;
                return param;
            }

            case Tables.SkillActionType.UseSkill:
            {
                var param = new SkillActionUseSkillParam();
                if (!SkillParamParseUtility.TryGetOptionalString(rawParams, 0, out var useSkillKey))
                {
                    return null;
                }

                param.SkillKey = useSkillKey;
                return param;
            }

            default:
                Debug.LogError($"[SkillActionParamParser] Unsupported SkillActionType. skillKey={skillKey}, type={type}");
                return null;
        }
    }
}
