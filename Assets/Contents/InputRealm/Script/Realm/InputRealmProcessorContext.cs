public class InputRealmProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        if (entity?.GetAbility<InputRealmProcessorAbility>() == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        return true;
    }

    public void Reset()
    {
        Entity = null;
    }
}
