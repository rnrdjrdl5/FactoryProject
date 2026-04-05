public class TeamPopupInputLayerProcessor : BasePopupInputLayerProcessor
{
    protected override bool CanConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleTeam || tokenType == PhysicalInputTokenType.Back;
    }
}
