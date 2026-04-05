public static class GlobalInputCommandLogic
{
    public static bool TryGetInputActionType(PhysicalInputTokenType tokenType, out GlobalInputActionType inputActionType)
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
