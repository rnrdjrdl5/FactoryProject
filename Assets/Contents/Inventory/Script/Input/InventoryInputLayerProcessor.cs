public class InventoryInputLayerProcessor : BaseContentInputLayerProcessor<InventoryInputType>
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

    protected override bool TryMapContentInput(TokenInputContext tokenInput, out ContentInputContext<InventoryInputType> contentInput)
    {
        if (tokenInput.InputType == TokenInputType.Menu3 && tokenInput.RawContext.InputType == RawInputType.Started)
        {
            contentInput = new ContentInputContext<InventoryInputType>(InventoryInputType.Open, tokenInput);
            return true;
        }

        contentInput = default;
        return false;
    }

    protected override LayerResult ProcessContentInput(ContentInputContext<InventoryInputType> contentInput)
    {
        RefreshGlobalActionProcessor();
        if (globalActionProcessor == null)
        {
            return LayerResult.Pass;
        }

        switch (contentInput.InputType)
        {
            case InventoryInputType.Open:
                globalActionProcessor.OpenInventory(contentInput);
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
