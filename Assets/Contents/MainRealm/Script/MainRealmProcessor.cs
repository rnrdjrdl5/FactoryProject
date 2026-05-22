using System.Collections.Generic;
using UnityEngine;

public class MainRealmProcessor : Processor
{
    public Player GetClosestHostilePlayer(Player player, Vector3 centerPosition, float range)
    {
        return TargetSearchLogic.GetClosestHostilePlayer(Entity.GetChildren<Player>(), player, centerPosition, range);
    }

    public List<Player> GetHostilePlayersInRange(Player player, Vector3 centerPosition, float range)
    {
        return TargetSearchLogic.GetHostilePlayersInRange(Entity.GetChildren<Player>(), player, centerPosition, range);
    }

    public List<Player> GetPlayersInRange(Player player, Tables.FactionRelationType relationType, Vector3 centerPosition, float range)
    {
        var targetPlayers = new List<Player>();
        if (player == null)
        {
            return targetPlayers;
        }

        var rangeSqr = range * range;
        foreach (var targetPlayer in Entity.GetChildren<Player>())
        {
            if (targetPlayer == null || targetPlayer == player)
            {
                continue;
            }

            if (!FactionLogic.IsRelation(player, targetPlayer, relationType))
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
