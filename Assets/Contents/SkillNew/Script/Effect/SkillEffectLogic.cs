using System;
using UnityEngine;

public static class SkillEffectLogic
{
    const float EffectDuration = 0.6f;

    public static void Play(string skillEffectKey, Entity targetEntity)
    {
        if (string.IsNullOrWhiteSpace(skillEffectKey) || targetEntity == null)
        {
            return;
        }

        var objectPoolAbility = targetEntity.RootAbilitySet?.GetAbility<ObjectPoolAbility>();
        if (objectPoolAbility == null)
        {
            return;
        }

        var effectDefinition = LoadEffectDefinition(skillEffectKey);
        var effectPrefab = effectDefinition?.Prefab;
        if (effectPrefab == null)
        {
            return;
        }

        var effectObject = objectPoolAbility.AllocateGameObject(effectPrefab, targetEntity.transform);
        effectObject.transform.localPosition = Vector3.zero;
        effectObject.transform.localRotation = Quaternion.identity;
        effectObject.transform.localScale = Vector3.one;
        effectObject.SetActive(true);
    }

    static SkillEffectDefinition LoadEffectDefinition(string skillEffectKey)
    {
        try
        {
            return Realm.LoadResources<SkillEffectDefinition>(skillEffectKey);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to load skill effect definition. Key={skillEffectKey}, Error={ex.Message}");
            return null;
        }
    }
}
