public class TeamInputLayerProcessor : BasePopupInputLayerProcessor
{
    public override InputLayerType LayerType => InputLayerType.Team;

    protected override bool ShouldConsume(PhysicalInputTokenType tokenType)
    {
        return tokenType == PhysicalInputTokenType.ToggleTeam || tokenType == PhysicalInputTokenType.Back;
    }
}
