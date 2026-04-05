public class InventoryInputLayerProcessor : BasePopupInputLayerProcessor
{
    public override InputLayerType LayerType => InputLayerType.Inventory;

    protected override bool ShouldConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleInventory || tokenType == PhysicalInputTokenType.Back;
    }
}
