public class BrainInputProcessor : Processor, IBrainInputActionRequester
{
    public void RequestAction(InputActionType inputActionType)
    {
        var actionRequester = GetPlayerActionRequester();
        actionRequester?.RequestAction(new PerformInputActionRequest
        {
            InputActionType = inputActionType
        });
    }

    IBrainActionRequester GetPlayerActionRequester()
    {
        var mainRealm = Entry.RootRealm?.GetChild<MainRealm>();
        if (mainRealm == null)
        {
            return null;
        }

        foreach (var brain in mainRealm.GetChildren<Brain>())
        {
            if (brain == null || brain.IsAI)
            {
                continue;
            }

            var processorAbility = brain.GetAbility<BrainProcessorAbility>();
            if (processorAbility?.GetProcessor<BrainActionProcessor>() is IBrainActionRequester actionRequester)
            {
                return actionRequester;
            }
        }

        return null;
    }
}
