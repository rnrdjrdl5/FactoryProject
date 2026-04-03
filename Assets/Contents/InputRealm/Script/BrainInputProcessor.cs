public class BrainInputProcessor : Processor
{
    BrainActionProcessor brainActionProcessor;

    public override void Ready()
    {
        base.Ready();

        RefreshActionProcessor();
    }

    public override void Uninitialize()
    {
        brainActionProcessor = null;

        base.Uninitialize();
    }

    public void RequestAction(InputActionType inputActionType)
    {
        RefreshActionProcessor();

        brainActionProcessor?.RequestAction(new PerformInputActionRequest
        {
            InputActionType = inputActionType
        });
    }

    void RefreshActionProcessor()
    {
        if (brainActionProcessor != null)
        {
            return;
        }

        var mainRealm = Entry.RootRealm?.GetChild<MainRealm>();
        if (mainRealm == null)
        {
            return;
        }

        foreach (var brain in mainRealm.GetChildren<Brain>())
        {
            if (brain == null || brain.IsAI)
            {
                continue;
            }

            var processorAbility = brain.GetAbility<BrainProcessorAbility>();
            if (processorAbility?.GetProcessor<BrainActionProcessor>() is BrainActionProcessor playerBrainActionProcessor)
            {
                brainActionProcessor = playerBrainActionProcessor;
                return;
            }
        }
    }
}
