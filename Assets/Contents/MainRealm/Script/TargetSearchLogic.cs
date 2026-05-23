using System.Collections.Generic;
using UnityEngine;

public static class TargetSearchLogic
{
    public static Player GetClosestHostilePlayer(IEnumerable<Player> players, Player player, Vector3 centerPosition, float range)
    {
        var hostilePlayers = GetPlayersInRange(players, player, Tables.FactionRelationType.Hostile, centerPosition, range);
        Player closestHostilePlayer = null;
        var closestDistanceSqr = float.MaxValue;
        foreach (var targetPlayer in hostilePlayers)
        {
            var distanceSqr = MathUtils.DistanceSqr2D(targetPlayer.transform.position, centerPosition);
            if (distanceSqr > closestDistanceSqr)
            {
                continue;
            }

            closestDistanceSqr = distanceSqr;
            closestHostilePlayer = targetPlayer;
        }

        return closestHostilePlayer;
    }

    public static List<Player> GetHostilePlayersInRange(IEnumerable<Player> players, Player player, Vector3 centerPosition, float range)
    {
        return GetPlayersInRange(players, player, Tables.FactionRelationType.Hostile, centerPosition, range);
    }

    public static List<Player> GetFriendlyPlayersInRange(IEnumerable<Player> players, Player player, Vector3 centerPosition, float range)
    {
        return GetPlayersInRange(players, player, Tables.FactionRelationType.Friendly, centerPosition, range);
    }

    public static List<Player> GetPlayersInRange(
        IEnumerable<Player> players,
        Player player,
        Tables.FactionRelationType relationType,
        Vector3 centerPosition,
        float range)
    {
        var targetPlayers = new List<Player>();
        if (players == null || player == null)
        {
            return targetPlayers;
        }

        var rangeSqr = range * range;
        foreach (var targetPlayer in players)
        {
            if (!IsValidTarget(player, targetPlayer, relationType))
            {
                continue;
            }

            if (MathUtils.DistanceSqr2D(targetPlayer.transform.position, centerPosition) > rangeSqr)
            {
                continue;
            }

            targetPlayers.Add(targetPlayer);
        }

        return targetPlayers;
    }

    public static bool IsHostileTargetInRange(Player player, Player targetPlayer, float range)
    {
        if (!IsValidTarget(player, targetPlayer, Tables.FactionRelationType.Hostile))
        {
            return false;
        }

        if (float.IsInfinity(range))
        {
            return true;
        }

        return IsInRange(player.transform.position, targetPlayer.transform.position, range);
    }

    public static bool IsInRange(Vector3 centerPosition, Vector3 targetPosition, float range)
    {
        return MathUtils.DistanceSqr2D(targetPosition, centerPosition) <= range * range;
    }

    static bool IsValidTarget(Player player, Player targetPlayer, Tables.FactionRelationType relationType)
    {
        if (player == null || targetPlayer == null)
        {
            return false;
        }

        if (targetPlayer == player)
        {
            return relationType == Tables.FactionRelationType.Friendly;
        }

        return FactionLogic.IsRelation(player, targetPlayer, relationType);
    }
}
