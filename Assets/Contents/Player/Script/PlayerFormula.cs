using System;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class PlayerFormula
{
    readonly PlayerData playerData;
    IReadOnlyDictionary<string, Func<float>> variables;

    public IReadOnlyDictionary<string, Func<float>> Variables => variables ??= CreateVariables();

    public PlayerFormula(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public bool TryEvaluate(string formula, out float value)
    {
        value = 0f;
        if (string.IsNullOrWhiteSpace(formula))
        {
            return false;
        }

        try
        {
            if (formula.TryGetFormulaValue(out value, Variables))
            {
                return true;
            }

            Debug.LogError($"[PlayerFormula] Invalid formula format. formula={formula}, expectedPrefix=\": \"");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"[PlayerFormula] Failed to evaluate formula. formula={formula}, error={e.Message}");
            return false;
        }
    }

    IReadOnlyDictionary<string, Func<float>> CreateVariables()
    {
        var createdVariables = new Dictionary<string, Func<float>>(StringComparer.OrdinalIgnoreCase);

        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            var capturedStatType = statType;
            createdVariables[EnumLogic.GetStatName(statType)] = () => GetStatValue(capturedStatType);
        }

        return createdVariables;
    }

    float GetStatValue(StatType statType)
    {
        if (playerData?.Stat != null && playerData.Stat.TryGetStat(statType, out var value))
        {
            return value;
        }

        return 0f;
    }
}
