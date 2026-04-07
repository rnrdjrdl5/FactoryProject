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
