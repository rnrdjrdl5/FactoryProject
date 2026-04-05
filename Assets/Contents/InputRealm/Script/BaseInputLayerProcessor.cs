public abstract class BaseInputLayerProcessor : Processor, ILayerProcessor<InputLayerType, PhysicalInputTokenEvent>
{
    protected InputRealmProcessorAbility InputRealmProcessorAbility => inputRealmProcessorAbility;

    InputRealmProcessorAbility inputRealmProcessorAbility;

    public abstract InputLayerType LayerType { get; }

    public override void Ready()
    {
        base.Ready();

        inputRealmProcessorAbility = Entity.GetAbility<InputRealmProcessorAbility>();
        inputRealmProcessorAbility?.RegisterLayerProcessor(this);
    }

    public override void Uninitialize()
    {
        inputRealmProcessorAbility?.UnregisterLayerProcessor(this);
        inputRealmProcessorAbility = null;

        base.Uninitialize();
    }

    public abstract LayerResult ProcessInput(PhysicalInputTokenEvent input);
}
