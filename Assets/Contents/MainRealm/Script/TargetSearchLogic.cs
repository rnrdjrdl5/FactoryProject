using System.Collections.Generic;
using UnityEngine;

public static class TargetSearchLogic
{
    public static Player GetClosestHostilePlayer(IEnumerable<Player> players, Player player, Vector3 centerPosition, float range)
    {
        var hostilePlayers = GetHostilePlayersInRange(players, player, centerPosition, range);
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
        var hostilePlayers = new List<Player>();
        if (players == null || player == null)
        {
            return hostilePlayers;
        }

        var rangeSqr = range * range;
        foreach (var targetPlayer in players)
        {
            if (!IsValidHostileTarget(player, targetPlayer))
            {
                continue;
            }

            if (MathUtils.DistanceSqr2D(targetPlayer.transform.position, centerPosition) > rangeSqr)
            {
                continue;
            }

            hostilePlayers.Add(targetPlayer);
        }

        return hostilePlayers;
    }

    public static bool IsHostileTargetInRange(Player player, Player targetPlayer, float range)
    {
        if (!IsValidHostileTarget(player, targetPlayer))
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

    static bool IsValidHostileTarget(Player player, Player targetPlayer)
    {
        if (player == null || targetPlayer == null || targetPlayer == player)
        {
            return false;
        }

        return FactionLogic.IsHostile(player, targetPlayer);
    }
}
