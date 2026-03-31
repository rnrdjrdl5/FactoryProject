using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class SkillParamParseUtility
{
    public static bool TryGetOptionalString(
        IReadOnlyList<string> rawParams,
        int index,
        out string value)
    {
        value = null;
        if (!TryGetValue(rawParams, index, out var rawValue))
        {
            return true;
        }

        value = rawValue;
        return true;
    }

    public static bool TryGetOptionalFloat(
        IReadOnlyList<string> rawParams,
        int index,
        string skillKey,
        string paramName,
        out float? value)
    {
        value = null;
        if (!TryGetValue(rawParams, index, out var rawValue))
        {
            return true;
        }

        if (!float.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
        {
            Debug.LogError($"[SkillParamParser] Invalid float value. skillKey={skillKey}, param={paramName}, value={rawValue}");
            return false;
        }

        value = parsedValue;
        return true;
    }

    public static bool TryGetOptionalInt(
        IReadOnlyList<string> rawParams,
        int index,
        string skillKey,
        string paramName,
        out int? value)
    {
        value = null;
        if (!TryGetValue(rawParams, index, out var rawValue))
        {
            return true;
        }

        if (!int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            Debug.LogError($"[SkillParamParser] Invalid int value. skillKey={skillKey}, param={paramName}, value={rawValue}");
            return false;
        }

        value = parsedValue;
        return true;
    }

    public static bool TryGetOptionalEnum<TEnum>(
        IReadOnlyList<string> rawParams,
        int index,
        string skillKey,
        string paramName,
        out TEnum? value) where TEnum : struct
    {
        value = null;
        if (!TryGetValue(rawParams, index, out var rawValue))
        {
            return true;
        }

        if (!System.Enum.TryParse<TEnum>(rawValue, true, out var parsedValue))
        {
            Debug.LogError($"[SkillParamParser] Invalid enum value. skillKey={skillKey}, param={paramName}, value={rawValue}, enumType={typeof(TEnum).Name}");
            return false;
        }

        value = parsedValue;
        return true;
    }

    static bool TryGetValue(IReadOnlyList<string> rawParams, int index, out string value)
    {
        value = null;
        if (rawParams == null || index < 0 || index >= rawParams.Count)
        {
            return false;
        }

        var rawValue = rawParams[index];
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return false;
        }

        value = rawValue.Trim();
        return true;
    }
}
