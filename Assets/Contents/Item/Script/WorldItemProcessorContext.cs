public class WorldItemProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public WorldItemEquipProcessor WorldItemEquipProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<WorldItemProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        WorldItemEquipProcessor = ability.GetProcessor<WorldItemEquipProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        WorldItemEquipProcessor = null;
    }
}
