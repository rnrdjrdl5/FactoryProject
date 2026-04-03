public class GlobalInputCommandProcessor : Processor
{
    GlobalActionProcessor globalActionProcessor;

    public override void Ready()
    {
        base.Ready();

        RefreshActionProcessor();
    }

    public override void Uninitialize()
    {
        globalActionProcessor = null;

        base.Uninitialize();
    }

    public void RequestAction(GlobalInputActionType inputActionType)
    {
        RefreshActionProcessor();

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

    void RefreshActionProcessor()
    {
        if (globalActionProcessor != null)
        {
            return;
        }

        var mainRealm = Entry.RootRealm?.GetChild<MainRealm>();
        var globalRealm = mainRealm?.GetChild<GlobalRealm>();
        var processorAbility = globalRealm?.GetAbility<GlobalRealmProcessorAbility>();
        globalActionProcessor = processorAbility?.GetProcessor<GlobalActionProcessor>();
    }
}
