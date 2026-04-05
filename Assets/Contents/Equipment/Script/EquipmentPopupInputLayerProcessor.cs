public class EquipmentPopupInputLayerProcessor : BasePopupInputLayerProcessor
{
    protected override bool ShouldConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleEquipment || tokenType == PhysicalInputTokenType.Back;
    }
}
