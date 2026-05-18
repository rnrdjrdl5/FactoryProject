public class EquipmentPopupInputLayerProcessor : BasePopupTokenInputLayerProcessor
{
    protected override bool CanConsume(TokenInputType inputType)
    {
        return inputType == TokenInputType.Menu2 || inputType == TokenInputType.Cancel;
    }
}
