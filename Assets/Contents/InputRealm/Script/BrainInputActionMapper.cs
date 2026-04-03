public class BrainInputActionMapper : Processor, IPhysicalInputTokenRequester
{
    BrainInputProcessor brainInputProcessor;

    public override void Ready()
    {
        base.Ready();

        var processorAbility = Entity.GetAbility<InputRealmProcessorAbility>();
        brainInputProcessor = processorAbility?.GetProcessor<BrainInputProcessor>();
    }

    public override void Uninitialize()
    {
        brainInputProcessor = null;

        base.Uninitialize();
    }

    public void RequestTokenInput(PhysicalInputTokenEvent tokenInput)
    {
        if (!TryGetPlayerInputActionType(tokenInput.TokenType, out var inputActionType))
        {
            return;
        }

        brainInputProcessor?.RequestAction(inputActionType);
    }

    bool TryGetPlayerInputActionType(PhysicalInputTokenType tokenType, out InputActionType inputActionType)
    {
        switch (tokenType)
        {
            case PhysicalInputTokenType.Pick:
                inputActionType = InputActionType.Pick;
                return true;
            case PhysicalInputTokenType.PrimaryClick:
                inputActionType = InputActionType.MainAttack;
                return true;
            case PhysicalInputTokenType.SecondaryClick:
                inputActionType = InputActionType.SubAttack;
                return true;
            case PhysicalInputTokenType.Hotkey1:
                inputActionType = InputActionType.Skill1;
                return true;
            case PhysicalInputTokenType.Hotkey2:
                inputActionType = InputActionType.Skill2;
                return true;
            case PhysicalInputTokenType.Hotkey3:
                inputActionType = InputActionType.Skill3;
                return true;
            case PhysicalInputTokenType.Utility1:
                inputActionType = InputActionType.MainUtility;
                return true;
            case PhysicalInputTokenType.Utility2:
                inputActionType = InputActionType.SubUtility;
                return true;
            default:
                inputActionType = default;
                return false;
        }
    }

}
