public class GlobalTokenInputProcessor : Processor, IPhysicalInputTokenRequester
{
    IGlobalInputActionRequester globalInputActionRequester;

    public override void Ready()
    {
        base.Ready();

        var processorAbility = Entity.GetAbility<GlobalRealmProcessorAbility>();
        globalInputActionRequester = processorAbility?.GetProcessor<GlobalInputProcessor>();
    }

    public override void Uninitialize()
    {
        globalInputActionRequester = null;

        base.Uninitialize();
    }

    public void RequestTokenInput(PhysicalInputTokenEvent tokenInput)
    {
        if (!TryGetGlobalInputActionType(tokenInput.TokenType, out var inputActionType))
        {
            return;
        }

        globalInputActionRequester?.RequestAction(inputActionType);
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
