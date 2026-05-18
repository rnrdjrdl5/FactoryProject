public class TeamPopupInputLayerProcessor : BasePopupTokenInputLayerProcessor
{
    protected override bool CanConsume(TokenInputType inputType)
    {
        return inputType == TokenInputType.Menu1 || inputType == TokenInputType.Cancel;
    }
}
