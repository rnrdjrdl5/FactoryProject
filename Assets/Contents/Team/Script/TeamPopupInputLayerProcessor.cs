public class TeamPopupInputLayerProcessor : BasePopupInputLayerProcessor
{
    protected override bool ShouldConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleTeam || tokenType == PhysicalInputTokenType.Back;
    }
}
