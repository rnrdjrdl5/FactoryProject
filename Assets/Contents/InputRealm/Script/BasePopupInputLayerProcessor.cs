public abstract class BasePopupInputLayerProcessor : BaseInputLayerProcessor
{
    PanelAbility panelAbility;

    public override void Ready()
    {
        base.Ready();

        var mainRealm = Entity.GetParent<MainRealm>();
        var globalRealm = mainRealm?.GetChild<GlobalRealm>();
        panelAbility = globalRealm?.GetAbility<PanelAbility>();
    }

    public override void Uninitialize()
    {
        panelAbility = null;

        base.Uninitialize();
    }

    public override LayerResult ProcessInput(PhysicalInputTokenEvent input)
    {
        if (ShouldConsume(input.TokenType))
        {
            CloseCurrentPopup();
            return LayerResult.Consume;
        }

        return LayerResult.Block;
    }

    protected abstract bool ShouldConsume(PhysicalInputTokenType tokenType);

    void CloseCurrentPopup()
    {
        panelAbility?.Panel?.Close();
    }
}
