public class EquipmentPopupProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public EquipmentPopupProcessor EquipmentPopupProcessor { get; private set; }
    public EquipmentPopupInputLayerProcessor EquipmentPopupInputLayerProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<EquipmentPopupProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        EquipmentPopupProcessor = ability.GetProcessor<EquipmentPopupProcessor>();
        EquipmentPopupInputLayerProcessor = ability.GetProcessor<EquipmentPopupInputLayerProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        EquipmentPopupProcessor = null;
        EquipmentPopupInputLayerProcessor = null;
    }
}
