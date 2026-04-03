public class PhysicalTokenRouterProcessor : Processor, IPhysicalInputTokenRequester
{
    PhysicalTokenEmitterAbility physicalTokenEmitterAbility;
    GlobalInputCommandMapper globalInputActionMapper;
    BrainInputCommandMapper brainInputActionMapper;

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
        physicalTokenEmitterAbility.SetInputStateData(Entity.GetEntityData<PhysicalInputStateData>());

        var processorAbility = Entity.GetAbility<InputRealmProcessorAbility>();
        globalInputActionMapper = processorAbility?.GetProcessor<GlobalInputCommandMapper>();
        brainInputActionMapper = processorAbility?.GetProcessor<BrainInputCommandMapper>();
    }

    public override void Uninitialize()
    {
        physicalTokenEmitterAbility?.SetTokenRequester(null);
        physicalTokenEmitterAbility?.SetInputBindingData(null);
        physicalTokenEmitterAbility?.SetInputStateData(null);

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
