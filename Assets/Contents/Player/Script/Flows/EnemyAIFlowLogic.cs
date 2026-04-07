using UnityEngine;

public static class EnemyAIFlowLogic
{
    const float DefaultDetectionRange = 6f;
    const float DefaultAttackRange = 1.5f;
    const float TargetLostRangeOffset = 2f;
    const float FollowDistanceRatio = 0.9f;

    public static Player GetControlledPlayer(ProcessorFlow flow)
    {
        var brain = flow?.Entity as Brain;
        return brain?.Controll as Player;
    }

    public static MainRealmProcessor GetMainRealmProcessor(ProcessorFlow flow)
    {
        var processorAbility = flow?.Realm?.GetAbility<MainRealmProcessorAbility>();
        return processorAbility?.GetProcessor<MainRealmProcessor>();
    }

    public static float GetMainAttackRange(Player player)
    {
        if (!TryGetMainAttackSkill(player, out var skillData))
        {
            return DefaultAttackRange;
        }

        return Mathf.Max(DefaultAttackRange, skillData.range);
    }

    public static float GetDetectionRange(Player player)
    {
        return Mathf.Max(DefaultDetectionRange, GetMainAttackRange(player) * 2f);
    }

    public static float GetTargetLostRange(Player player)
    {
        return GetDetectionRange(player) + TargetLostRangeOffset;
    }

    public static Player FindClosestHostileTarget(ProcessorFlow flow, Player player, float range)
    {
        var realmProcessor = GetMainRealmProcessor(flow);
        return realmProcessor?.GetClosestHostilePlayer(player, player.transform.position, range);
    }

    public static bool IsTargetValid(Player ownerPlayer, Player targetPlayer, float range = Mathf.Infinity)
    {
        if (ownerPlayer == null || targetPlayer == null)
        {
            return false;
        }

        if (!FactionLogic.IsHostile(ownerPlayer, targetPlayer))
        {
            return false;
        }

        if (float.IsInfinity(range))
        {
            return true;
        }

        var distanceSqr = ((Vector2)ownerPlayer.transform.position - (Vector2)targetPlayer.transform.position).sqrMagnitude;
        return distanceSqr <= range * range;
    }

    public static void SyncFollowDistanceToAttackRange(Player player, PlayerFollowAbility followAbility)
    {
        if (player == null || followAbility == null)
        {
            return;
        }

        followAbility.SetFollowDistance(GetMainAttackRange(player) * FollowDistanceRatio);
    }

    static bool TryGetMainAttackSkill(Player player, out Tables.Skill skillData)
    {
        skillData = null;

        var playerData = player?.GetEntityData<PlayerData>();
        if (!InputActionSkillLogic.TryGetSkillKey(playerData, InputActionType.MainAttack, out var skillKey))
        {
            return false;
        }

        skillData = Tables.Skill.Get(skillKey);
        return skillData != null;
    }
}
