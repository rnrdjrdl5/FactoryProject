public class MainStorageProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public MainStorageProcessor MainStorageProcessor { get; private set; }
    public MainStorageSynergeProcessor MainStorageSynergeProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<MainStorageProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        MainStorageProcessor = ability.GetProcessor<MainStorageProcessor>();
        MainStorageSynergeProcessor = ability.GetProcessor<MainStorageSynergeProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        MainStorageProcessor = null;
        MainStorageSynergeProcessor = null;
    }
}
