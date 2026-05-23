public class GlobalRealmProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public GlobalActionProcessor GlobalActionProcessor { get; private set; }
    public GlobalTeamProcessor GlobalTeamProcessor { get; private set; }
    public PlayerInputLayerProcessor PlayerInputLayerProcessor { get; private set; }
    public TeamInputLayerProcessor TeamInputLayerProcessor { get; private set; }
    public EquipmentInputLayerProcessor EquipmentInputLayerProcessor { get; private set; }
    public InventoryInputLayerProcessor InventoryInputLayerProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<GlobalRealmProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        GlobalActionProcessor = ability.GetProcessor<GlobalActionProcessor>();
        GlobalTeamProcessor = ability.GetProcessor<GlobalTeamProcessor>();
        PlayerInputLayerProcessor = ability.GetProcessor<PlayerInputLayerProcessor>();
        TeamInputLayerProcessor = ability.GetProcessor<TeamInputLayerProcessor>();
        EquipmentInputLayerProcessor = ability.GetProcessor<EquipmentInputLayerProcessor>();
        InventoryInputLayerProcessor = ability.GetProcessor<InventoryInputLayerProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        GlobalActionProcessor = null;
        GlobalTeamProcessor = null;
        PlayerInputLayerProcessor = null;
        TeamInputLayerProcessor = null;
        EquipmentInputLayerProcessor = null;
        InventoryInputLayerProcessor = null;
    }
}
