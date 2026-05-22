public class InventoryProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public InventoryProcessor InventoryProcessor { get; private set; }
    public InventoryPopupInputLayerProcessor InventoryPopupInputLayerProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<InventoryProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        InventoryProcessor = ability.GetProcessor<InventoryProcessor>();
        InventoryPopupInputLayerProcessor = ability.GetProcessor<InventoryPopupInputLayerProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        InventoryProcessor = null;
        InventoryPopupInputLayerProcessor = null;
    }
}
