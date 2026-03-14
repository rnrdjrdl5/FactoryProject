using System;
using UnityEngine;

[Serializable]
public class AccumulatingRollData : IEntityData
{
    public event Action OnChanged;
    
    float minRange = 0f;
    float maxRange = 100f;
    float baseThreshold = 1f;
    float increment = 1f;

    float currentThreshold;

    public float MaxRange
    {
        get => maxRange;
        set => maxRange = value;
    }

    public float MinRange
    {
        get => minRange;
        set => minRange = value;
    }

    public float BaseThreshold
    {
        get => baseThreshold;
        set => baseThreshold = value;
    }

    public float Increment
    {
        get => Mathf.Max(0f, increment);
        set => increment = value;
    }

    public float CurrentThreshold => currentThreshold;

    public void Initialize(IInitData initData = null)
    {
        ResetAccumulation();
    }

    public void Uninitialize()
    {
    }

    public bool TryRoll(float rollValue)
    {
        var min = minRange;
        var max = maxRange;
        if (MathUtils.Roll(min, max, currentThreshold))
        {
            currentThreshold = baseThreshold;

            return true;
        }

        currentThreshold = Mathf.Min(max, currentThreshold + Mathf.Max(0f, increment));
        return false;
    }

    public void ResetAccumulation()
    {
        var min = minRange;
        var max = maxRange;
        currentThreshold = Mathf.Clamp(baseThreshold, min, max);
    }
}
