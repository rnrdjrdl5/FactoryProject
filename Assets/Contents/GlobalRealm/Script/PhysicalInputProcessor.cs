public class PhysicalInputProcessor : Processor, IPhysicalInputTokenRequester
{
    PhysicalInputAbility physicalInputAbility;
    PhysicalInputBindingData physicalInputBindingData;
    IPhysicalInputTokenRequester globalTokenInputRequester;
    IPhysicalInputTokenRequester brainTokenInputRequester;

    public override void Ready()
    {
        base.Ready();

        physicalInputAbility = Entity.GetAbility<PhysicalInputAbility>();
        if (physicalInputAbility == null)
        {
            return;
        }

        physicalInputBindingData = Entity.GetEntityData<PhysicalInputBindingData>();
        var processorAbility = Entity.GetAbility<GlobalRealmProcessorAbility>();
        globalTokenInputRequester = processorAbility?.GetProcessor<GlobalTokenInputProcessor>();
        brainTokenInputRequester = processorAbility?.GetProcessor<BrainTokenInputProcessor>();

        physicalInputAbility.SetInputBindingData(physicalInputBindingData);
        physicalInputAbility.SetTokenRequester(this);
    }

    public override void Uninitialize()
    {
        physicalInputAbility?.SetTokenRequester(null);
        physicalInputAbility?.SetInputBindingData(null);

        physicalInputAbility = null;
        physicalInputBindingData = null;
        globalTokenInputRequester = null;
        brainTokenInputRequester = null;

        base.Uninitialize();
    }

    public void RequestTokenInput(PhysicalInputTokenEvent tokenInput)
    {
        globalTokenInputRequester?.RequestTokenInput(tokenInput);
        brainTokenInputRequester?.RequestTokenInput(tokenInput);
    }
}
