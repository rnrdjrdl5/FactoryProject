public class GlobalInputCommandMapper : Processor, IPhysicalInputTokenRequester
{
    GlobalInputCommandProcessor globalInputProcessor;

    public override void Ready()
    {
        base.Ready();

        var processorAbility = Entity.GetAbility<InputRealmProcessorAbility>();
        globalInputProcessor = processorAbility?.GetProcessor<GlobalInputCommandProcessor>();
    }

    public override void Uninitialize()
    {
        globalInputProcessor = null;

        base.Uninitialize();
    }

    public void RequestTokenInput(PhysicalInputTokenEvent tokenInput)
    {
        if (!TryGetGlobalInputActionType(tokenInput.TokenType, out var inputActionType))
        {
            return;
        }

        globalInputProcessor?.RequestAction(inputActionType);
    }

    bool TryGetGlobalInputActionType(PhysicalInputTokenType tokenType, out GlobalInputActionType inputActionType)
    {
        switch (tokenType)
        {
            case PhysicalInputTokenType.ToggleTeam:
                inputActionType = GlobalInputActionType.OpenTeam;
                return true;
            case PhysicalInputTokenType.ToggleEquipment:
                inputActionType = GlobalInputActionType.OpenEquipment;
                return true;
            case PhysicalInputTokenType.ToggleInventory:
                inputActionType = GlobalInputActionType.OpenInventory;
                return true;
            default:
                inputActionType = default;
                return false;
        }
    }
}
