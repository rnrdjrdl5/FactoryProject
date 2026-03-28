using System.Collections.Generic;
using UnityEngine;

public class MainRealmProcessor : Processor
{
    public (Brain brain, Player player) CreateBrainAndPlayer(Entity ownerEntity, string brainPath, string playerPath, PlayerInitData playerInitData)
    {
        var brain = ownerEntity.AddEntity<Brain>(brainPath);
        var player = ownerEntity.AddEntity<Player>(playerPath, playerInitData);
        brain.AttachControll(player);

        return (brain, player);
    }

    public Player GetClosestHostilePlayer(Player player, float range)
    {
        if (player == null)
        {
            return null;
        }

        Player closestHostilePlayer = null;
        var closestDistanceSqr = range * range;
        foreach (var targetPlayer in Realm.GetChildren<Player>())
        {
            if (targetPlayer == null || targetPlayer == player)
            {
                continue;
            }

            if (!FactionLogic.IsHostile(player, targetPlayer))
            {
                continue;
            }

            var distanceSqr = ((Vector2)targetPlayer.transform.position - (Vector2)player.transform.position).sqrMagnitude;
            if (distanceSqr > closestDistanceSqr)
            {
                continue;
            }

            closestDistanceSqr = distanceSqr;
            closestHostilePlayer = targetPlayer;
        }

        return closestHostilePlayer;
    }

    public List<Player> GetHostilePlayersInRange(Player player, Vector3 centerPosition, float range)
    {
        var hostilePlayers = new List<Player>();
        if (player == null)
        {
            return hostilePlayers;
        }

        var rangeSqr = range * range;
        foreach (var targetPlayer in Realm.GetChildren<Player>())
        {
            if (targetPlayer == null || targetPlayer == player)
            {
                continue;
            }

            if (!FactionLogic.IsHostile(player, targetPlayer))
            {
                continue;
            }

            var distanceSqr = ((Vector2)targetPlayer.transform.position - (Vector2)centerPosition).sqrMagnitude;
            if (distanceSqr > rangeSqr)
            {
                continue;
            }

            hostilePlayers.Add(targetPlayer);
        }

        return hostilePlayers;
    }

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        var spawnerInitData = new SpawnerInitData
        {
            SpawnerKey = Tables.TablesKey.Spawner_Test
        };
        
        var spawner = Realm.AddEntity<Spawner>(Spawner.PrefabName, spawnerInitData);
    }
}
