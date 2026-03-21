public class MainRealmProcessor : Processor
{
    public (Brain brain, Player player) CreateBrainAndPlayer(Entity ownerEntity, string brainPath, string playerPath, PlayerInitData playerInitData)
    {
        var brain = ownerEntity.AddEntity<Brain>(brainPath);
        var player = ownerEntity.AddEntity<Player>(playerPath, playerInitData);
        brain.AttachControll(player);

        return (brain, player);
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