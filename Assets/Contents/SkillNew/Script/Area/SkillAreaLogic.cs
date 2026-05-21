using System.Collections.Generic;
using UnityEngine;

public static class SkillAreaLogic
{
    public static List<Entity> GetTargetEntities(SkillContext skillContext, MainRealmProcessor mainRealmProcessor)
    {
        if (skillContext?.OriginCaster is not Player originCaster || skillContext.SkillData == null)
        {
            return new List<Entity>();
        }

        switch (skillContext.SkillData.ParsedAreaParam)
        {
            case SkillAreaCircleParam circleParam:
                return GetCircleTargetEntities(skillContext, originCaster, circleParam, mainRealmProcessor);

            default:
                return new List<Entity>();
        }
    }

    static List<Entity> GetCircleTargetEntities(
        SkillContext skillContext,
        Player originCaster,
        SkillAreaCircleParam circleParam,
        MainRealmProcessor mainRealmProcessor)
    {
        if (circleParam?.Radius == null)
        {
            return new List<Entity>();
        }

        var centerPosition = GetCircleCenterPosition(skillContext, originCaster);
        var targetPlayers = mainRealmProcessor?.GetPlayersInRange(
            originCaster,
            skillContext.SkillData.skillTargetType,
            centerPosition,
            circleParam.Radius.Value);
        if (targetPlayers == null || targetPlayers.Count == 0)
        {
            return new List<Entity>();
        }

        var targetEntities = new List<Entity>(targetPlayers.Count);
        foreach (var targetPlayer in targetPlayers)
        {
            targetEntities.Add(targetPlayer);
        }

        return targetEntities;
    }

    static Vector3 GetCircleCenterPosition(SkillContext skillContext, Player originCaster)
    {
        if (skillContext.SkillData.skillTargetType == Tables.FactionRelationType.Friendly)
        {
            return originCaster.transform.position;
        }

        if (skillContext.TargetPosition != null)
        {
            return skillContext.TargetPosition.Value;
        }

        return originCaster.transform.position;
    }
}
