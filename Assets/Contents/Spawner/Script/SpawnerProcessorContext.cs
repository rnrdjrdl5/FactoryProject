public class SpawnerProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public SpawnerProcessor SpawnerProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<SpawnerProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        SpawnerProcessor = ability.GetProcessor<SpawnerProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        SpawnerProcessor = null;
    }
}
