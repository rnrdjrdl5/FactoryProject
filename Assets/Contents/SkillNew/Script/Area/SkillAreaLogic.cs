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
                return GetCircleTargetEntities(originCaster, skillContext.TargetPosition, circleParam, mainRealmProcessor);

            default:
                return new List<Entity>();
        }
    }

    static List<Entity> GetCircleTargetEntities(
        Player originCaster,
        Vector3? targetPosition,
        SkillAreaCircleParam circleParam,
        MainRealmProcessor mainRealmProcessor)
    {
        if (targetPosition == null || circleParam?.Radius == null)
        {
            return new List<Entity>();
        }

        var targetPlayers = mainRealmProcessor?.GetHostilePlayersInRange(originCaster, targetPosition.Value, circleParam.Radius.Value);
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
}
