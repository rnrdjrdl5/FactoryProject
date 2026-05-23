public class PlayerProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public PlayerProcessor PlayerProcessor { get; private set; }
    public PlayerBuffProcessor PlayerBuffProcessor { get; private set; }
    public PlayerHpProcessor PlayerHpProcessor { get; private set; }
    public DropItemProcessor DropItemProcessor { get; private set; }
    public PlayerPickProcessor PlayerPickProcessor { get; private set; }
    public PlayerEquipProcessor PlayerEquipProcessor { get; private set; }
    public PlayerModelProcessor PlayerModelProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<PlayerProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        PlayerProcessor = ability.GetProcessor<PlayerProcessor>();
        PlayerBuffProcessor = ability.GetProcessor<PlayerBuffProcessor>();
        PlayerHpProcessor = ability.GetProcessor<PlayerHpProcessor>();
        DropItemProcessor = ability.GetProcessor<DropItemProcessor>();
        PlayerPickProcessor = ability.GetProcessor<PlayerPickProcessor>();
        PlayerEquipProcessor = ability.GetProcessor<PlayerEquipProcessor>();
        PlayerModelProcessor = ability.GetProcessor<PlayerModelProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        PlayerProcessor = null;
        PlayerBuffProcessor = null;
        PlayerHpProcessor = null;
        DropItemProcessor = null;
        PlayerPickProcessor = null;
        PlayerEquipProcessor = null;
        PlayerModelProcessor = null;
    }
}
