public class PhysicalTokenRouterProcessor : Processor, IPhysicalInputTokenRequester
{
    PhysicalTokenEmitterAbility physicalTokenEmitterAbility;
    GlobalInputActionMapper globalInputActionMapper;
    BrainInputActionMapper brainInputActionMapper;

    public override void Ready()
    {
        base.Ready();

        physicalTokenEmitterAbility = Entity.GetAbility<PhysicalTokenEmitterAbility>();
        if (physicalTokenEmitterAbility == null)
        {
            return;
        }
        
        physicalTokenEmitterAbility.SetTokenRequester(this);

        var physicalInputBindingData = Entity.GetEntityData<PhysicalInputBindingData>();
        physicalTokenEmitterAbility.SetInputBindingData(physicalInputBindingData);

        var processorAbility = Entity.GetAbility<InputRealmProcessorAbility>();
        globalInputActionMapper = processorAbility?.GetProcessor<GlobalInputActionMapper>();
        brainInputActionMapper = processorAbility?.GetProcessor<BrainInputActionMapper>();
    }

    public override void Uninitialize()
    {
        physicalTokenEmitterAbility?.SetTokenRequester(null);
        physicalTokenEmitterAbility?.SetInputBindingData(null);

        physicalTokenEmitterAbility = null;
        globalInputActionMapper = null;
        brainInputActionMapper = null;

        base.Uninitialize();
    }

    public void RequestTokenInput(PhysicalInputTokenEvent tokenInput)
    {
        globalInputActionMapper?.RequestTokenInput(tokenInput);
        brainInputActionMapper?.RequestTokenInput(tokenInput);
    }
}
