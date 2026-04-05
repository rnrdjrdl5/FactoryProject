public abstract class BaseInputLayerProcessor : Processor, ILayerProcessor<PhysicalInputTokenEvent>
{
    protected InputRealmProcessorAbility InputRealmProcessorAbility => inputRealmProcessorAbility;

    InputRealmProcessorAbility inputRealmProcessorAbility;

    public override void Ready()
    {
        base.Ready();

        inputRealmProcessorAbility = ResolveInputRealmProcessorAbility();
        inputRealmProcessorAbility?.PushLayer(this);
    }

    public override void Uninitialize()
    {
        inputRealmProcessorAbility?.RemoveLayer(this);
        inputRealmProcessorAbility = null;

        base.Uninitialize();
    }

    protected virtual InputRealmProcessorAbility ResolveInputRealmProcessorAbility()
    {
        var result = Entity.GetAbility<InputRealmProcessorAbility>();
        if (result != null)
        {
            return result;
        }

        var inputRealm = Entity.GetFromRoot<InputRealm>();
        return inputRealm?.GetAbility<InputRealmProcessorAbility>();
    }

    public abstract LayerResult ProcessInput(PhysicalInputTokenEvent input);
}
