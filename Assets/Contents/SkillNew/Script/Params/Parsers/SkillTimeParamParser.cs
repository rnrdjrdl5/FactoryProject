using System.Collections.Generic;
using UnityEngine;

public static class SkillTimeParamParser
{
    public static ISkillTimeParam Parse(Tables.SkillTimeType type, IReadOnlyList<string> rawParams, string skillKey)
    {
        switch (type)
        {
            case Tables.SkillTimeType.Instance:
                return new SkillTimeInstanceParam();

            case Tables.SkillTimeType.Tick:
            {
                var param = new SkillTimeTickParam();
                if (!SkillParamParseUtility.TryGetOptionalFloat(rawParams, 0, skillKey, "intervalSeconds", out var intervalSeconds))
                {
                    return null;
                }

                if (!SkillParamParseUtility.TryGetOptionalInt(rawParams, 1, skillKey, "repeatCount", out var repeatCount))
                {
                    return null;
                }

                param.IntervalSeconds = intervalSeconds;
                param.RepeatCount = repeatCount;
                return param;
            }

            default:
                Debug.LogError($"[SkillTimeParamParser] Unsupported SkillTimeType. skillKey={skillKey}, type={type}");
                return null;
        }
    }
}
