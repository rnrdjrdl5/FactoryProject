public class GlobalInputProcessor : Processor, IGlobalInputActionRequester
{
    GlobalInputAbility globalInputAbility;
    GlobalInputBindingData globalInputBindingData;
    GlobalProcessor globalProcessor;

    public override void Ready()
    {
        base.Ready();

        globalInputAbility = Entity.GetAbility<GlobalInputAbility>();
        if (globalInputAbility == null)
        {
            return;
        }

        globalInputBindingData = Entity.GetEntityData<GlobalInputBindingData>();

        var processorAbility = Entity.GetAbility<GlobalRealmProcessorAbility>();
        globalProcessor = processorAbility?.GetProcessor<GlobalProcessor>();

        globalInputAbility.SetInputBindingData(globalInputBindingData);
        globalInputAbility.SetActionRequester(this);
    }

    public override void Uninitialize()
    {
        globalInputAbility?.SetActionRequester(null);
        globalInputAbility?.SetInputBindingData(null);

        globalInputAbility = null;
        globalInputBindingData = null;
        globalProcessor = null;

        base.Uninitialize();
    }

    public void RequestAction(GlobalInputActionType inputActionType)
    {
        switch (inputActionType)
        {
            case GlobalInputActionType.OpenTeam:
                globalProcessor?.OpenTeam();
                break;
            case GlobalInputActionType.OpenEquipment:
                globalProcessor?.OpenEquipment();
                break;
            case GlobalInputActionType.OpenInventory:
                globalProcessor?.OpenInventory();
                break;
        }
    }
}
