public static class InputActionSystemLogic
{
    public static bool TryGetSystemActionType(InputActionType inputActionType, out SystemActionType systemActionType)
    {
        switch (inputActionType)
        {
            case InputActionType.Pick:
                systemActionType = SystemActionType.Pick;
                return true;
            default:
                systemActionType = default;
                return false;
        }
    }
}
