namespace Tables
{
    public partial class Spawner : IKey, IPrefabPath
    {
        public string GetSpawnPlayerKey()
        {
            var index = MathUtils.SelectRandomIndexByWeight(spawnPercent);
            return spawnPlayerKey[index];
        }
    }
}
