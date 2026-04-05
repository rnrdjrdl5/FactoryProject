public class EquipmentPopupInputLayerProcessor : BasePopupInputLayerProcessor
{
    protected override bool CanConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleEquipment || tokenType == PhysicalInputTokenType.Back;
    }
}
