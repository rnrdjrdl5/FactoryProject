public static class BrainInputCommandLogic
{
    public static bool TryGetInputActionType(PhysicalInputTokenType tokenType, out InputActionType inputActionType)
    {
        switch (tokenType)
        {
            case PhysicalInputTokenType.AxisInputChanged:
                inputActionType = InputActionType.Move;
                return true;
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
