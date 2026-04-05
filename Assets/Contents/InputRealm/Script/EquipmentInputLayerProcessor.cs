public class EquipmentInputLayerProcessor : BasePopupInputLayerProcessor
{
    public override InputLayerType LayerType => InputLayerType.Equipment;

    protected override bool ShouldConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleEquipment || tokenType == PhysicalInputTokenType.Back;
    }
}
