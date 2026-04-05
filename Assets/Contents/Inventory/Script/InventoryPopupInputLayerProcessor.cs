public class InventoryPopupInputLayerProcessor : BasePopupInputLayerProcessor
{
    protected override bool ShouldConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleInventory || tokenType == PhysicalInputTokenType.Back;
    }
}
