using System.Collections.Generic;
using UnityEngine;

public static class SkillAreaLogic
{
    public static List<Entity> GetTargetEntities(SkillContext skillContext, MainRealmProcessor mainRealmProcessor)
    {
        if (skillContext?.Caster is not Player caster || skillContext.SkillData == null)
        {
            return new List<Entity>();
        }

        switch (skillContext.SkillData.ParsedAreaParam)
        {
            case SkillAreaCircleParam circleParam:
                return GetCircleTargetEntities(caster, skillContext.TargetPosition, circleParam, mainRealmProcessor);

            default:
                return new List<Entity>();
        }
    }

    static List<Entity> GetCircleTargetEntities(
        Player caster,
        Vector3? targetPosition,
        SkillAreaCircleParam circleParam,
        MainRealmProcessor mainRealmProcessor)
    {
        if (targetPosition == null || circleParam?.Radius == null)
        {
            return new List<Entity>();
        }

        var targetPlayers = mainRealmProcessor?.GetHostilePlayersInRange(caster, targetPosition.Value, circleParam.Radius.Value);
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
