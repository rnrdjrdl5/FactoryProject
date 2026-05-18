public class InventoryPopupInputLayerProcessor : BasePopupTokenInputLayerProcessor
{
    protected override bool CanConsume(TokenInputType inputType)
    {
        return inputType == TokenInputType.Menu3 || inputType == TokenInputType.Cancel;
    }
}
