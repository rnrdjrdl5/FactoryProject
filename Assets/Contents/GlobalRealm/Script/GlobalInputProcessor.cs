public class GlobalInputProcessor : Processor, IGlobalInputActionRequester
{
    GlobalActionProcessor globalActionProcessor;

    public override void Ready()
    {
        base.Ready();

        var processorAbility = Entity.GetAbility<GlobalRealmProcessorAbility>();
        globalActionProcessor = processorAbility?.GetProcessor<GlobalActionProcessor>();
    }

    public override void Uninitialize()
    {
        globalActionProcessor = null;

        base.Uninitialize();
    }

    public void RequestAction(GlobalInputActionType inputActionType)
    {
        switch (inputActionType)
        {
            case GlobalInputActionType.OpenTeam:
                globalActionProcessor?.OpenTeam();
                break;
            case GlobalInputActionType.OpenEquipment:
                globalActionProcessor?.OpenEquipment();
                break;
            case GlobalInputActionType.OpenInventory:
                globalActionProcessor?.OpenInventory();
                break;
        }
    }
}
