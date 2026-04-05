public class InventoryPopupInputLayerProcessor : BasePopupInputLayerProcessor
{
    protected override bool CanConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleInventory || tokenType == PhysicalInputTokenType.Back;
    }
}
