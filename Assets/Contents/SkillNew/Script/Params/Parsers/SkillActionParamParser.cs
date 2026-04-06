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
                if (!SkillParamParseUtility.TryGetOptionalString(rawParams, 0, out var amountRaw))
                {
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(amountRaw))
                {
                    if (float.TryParse(amountRaw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var amount))
                    {
                        param.Amount = amount;
                    }
                    else
                    {
                        param.AmountFormula = amountRaw;
                    }
                }

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
                
                if (!SkillParamParseUtility.TryGetOptionalString(rawParams, 2, out var projectileKey))
                {
                    return null;
                }

                param.ProjectileKey = projectileKey;
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

            case Tables.SkillActionType.ApplyStat:
            {
                var param = new SkillActionApplyStatParam();
                if (!SkillParamParseUtility.TryGetOptionalEnum<Tables.StatType>(rawParams, 0, skillKey, "statType", out var statType))
                {
                    return null;
                }

                if (!SkillParamParseUtility.TryGetOptionalInt(rawParams, 1, skillKey, "value", out var value))
                {
                    return null;
                }

                param.StatType = statType;
                param.Value = value;
                return param;
            }

            case Tables.SkillActionType.AddBuff:
            {
                var param = new SkillActionAddBuffParam();
                if (!SkillParamParseUtility.TryGetOptionalString(rawParams, 0, out var buffKey))
                {
                    return null;
                }

                if (Tables.Buff.Get(buffKey) == null)
                {
                    Debug.LogError($"[SkillActionParamParser] Invalid buffKey. skillKey={skillKey}, buffKey={buffKey}");
                    return null;
                }

                param.BuffKey = buffKey;
                return param;
            }

            default:
                Debug.LogError($"[SkillActionParamParser] Unsupported SkillActionType. skillKey={skillKey}, type={type}");
                return null;
        }
    }
}
