public class EquipmentInputLayerProcessor : BaseContentInputLayerProcessor<EquipmentInputType>
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

    protected override bool TryMapContentInput(TokenInputContext tokenInput, out ContentInputContext<EquipmentInputType> contentInput)
    {
        if (tokenInput.InputType == TokenInputType.Menu2 && tokenInput.RawContext.InputType == RawInputType.Started)
        {
            contentInput = new ContentInputContext<EquipmentInputType>(EquipmentInputType.Open, tokenInput);
            return true;
        }

        contentInput = default;
        return false;
    }

    protected override LayerResult ProcessContentInput(ContentInputContext<EquipmentInputType> contentInput)
    {
        RefreshGlobalActionProcessor();
        if (globalActionProcessor == null)
        {
            return LayerResult.Pass;
        }

        switch (contentInput.InputType)
        {
            case EquipmentInputType.Open:
                globalActionProcessor.OpenEquipment(contentInput);
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
