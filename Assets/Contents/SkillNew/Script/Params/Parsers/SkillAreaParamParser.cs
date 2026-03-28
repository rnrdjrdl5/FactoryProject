using System.Collections.Generic;
using Tables;
using UnityEngine;

public static class SkillAreaParamParser
{
    public static ISkillAreaParam Parse(Tables.SkillAreaType type, IReadOnlyList<string> rawParams, string skillKey)
    {
        switch (type)
        {
            case Tables.SkillAreaType.Circle:
            {
                var param = new SkillAreaCircleParam();
                if (!SkillParamParseUtility.TryGetOptionalFloat(rawParams, 0, skillKey, "radius", out var radius))
                {
                    return null;
                }

                param.Radius = radius;
                return param;
            }
            
            case SkillAreaType.None:
            {
                return null;
            }

            default:
                Debug.LogError($"[SkillAreaParamParser] Unsupported SkillAreaType. skillKey={skillKey}, type={type}");
                return null;
        }
    }
}
