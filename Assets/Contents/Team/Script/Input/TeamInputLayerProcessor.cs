public class TeamInputLayerProcessor : BaseContentInputLayerProcessor<TeamInputType>
{
    GlobalActionProcessor globalActionProcessor;

    public override void Ready()
    {
        base.Ready();

        RefreshGlobalActionProcessor();
    }

    public override void Uninitialize()
    {
        globalActionProcessor = null;

        base.Uninitialize();
    }

    protected override bool TryMapContentInput(TokenInputContext tokenInput, out ContentInputContext<TeamInputType> contentInput)
    {
        if (tokenInput.InputType == TokenInputType.Menu1 && tokenInput.RawContext.InputType == RawInputType.Started)
        {
            contentInput = new ContentInputContext<TeamInputType>(TeamInputType.Open, tokenInput);
            return true;
        }

        contentInput = default;
        return false;
    }

    protected override LayerResult ProcessContentInput(ContentInputContext<TeamInputType> contentInput)
    {
        RefreshGlobalActionProcessor();
        if (globalActionProcessor == null)
        {
            return LayerResult.Pass;
        }

        switch (contentInput.InputType)
        {
            case TeamInputType.Open:
                globalActionProcessor.OpenTeam(contentInput);
                return LayerResult.Consume;
            default:
                return LayerResult.Pass;
        }
    }

    void RefreshGlobalActionProcessor()
    {
        if (globalActionProcessor != null)
        {
            return;
        }

        var globalRealm = Entity.GetFromRoot<GlobalRealm>();
        var processorAbility = globalRealm?.GetAbility<GlobalRealmProcessorAbility>();
        globalActionProcessor = processorAbility?.GetProcessor<GlobalActionProcessor>();
    }
}
