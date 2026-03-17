public class MainRealmProcessor : Processor
{
    public (Brain brain, Player player) CreateBrainAndEntity(Entity ownerEntity, string brainPath, string playerPath, IInitData brainData = null, IInitData playerData = null)
    {
        var brain = ownerEntity.AddEntity<Brain>(brainPath, brainData);
        var entity = ownerEntity.AddEntity<Player>(playerPath, playerData);
        brain.AttachControll(entity);

        return (brain, entity);
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