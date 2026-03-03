using System;
using UnityEngine;

public class CustomRandomAbility : Ability
{
    [SerializeField] int seed = 0;

    System.Random random;

    public int Seed => seed;
    public float Value => (float)EnsureRandom().NextDouble();

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        Reseed(seed);
    }

    public void Reseed(int newSeed)
    {
        seed = newSeed;
        random = new System.Random(seed);
    }

    public int Range(int minInclusive, int maxExclusive)
    {
        return EnsureRandom().Next(minInclusive, maxExclusive);
    }

    public float Range(float minInclusive, float maxInclusive)
    {
        if (Mathf.Approximately(minInclusive, maxInclusive))
        {
            return minInclusive;
        }

        var t = EnsureRandom().NextDouble();
        return (float)(minInclusive + (maxInclusive - minInclusive) * t);
    }

    public bool Chance(float probability)
    {
        return EnsureRandom().NextDouble() < Mathf.Clamp01(probability);
    }

    System.Random EnsureRandom()
    {
        if (random == null)
        {
            random = new System.Random(seed);
        }

        return random;
    }
}
