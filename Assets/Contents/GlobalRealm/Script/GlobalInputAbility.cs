using UnityEngine;

public class GlobalInputAbility : Ability
{
    IGlobalInputActionRequester actionRequester;
    GlobalInputBindingData inputBindingData;

    public void SetActionRequester(IGlobalInputActionRequester actionRequester)
    {
        this.actionRequester = actionRequester;
    }

    public void SetInputBindingData(GlobalInputBindingData inputBindingData)
    {
        this.inputBindingData = inputBindingData;
    }

    void Update()
    {
        TryRequestAction(KeyCode.F1);
        TryRequestAction(KeyCode.F2);
        TryRequestAction(KeyCode.I);
    }

    void TryRequestAction(KeyCode keyCode)
    {
        if (actionRequester == null || inputBindingData == null)
        {
            return;
        }

        if (!Input.GetKeyDown(keyCode))
        {
            return;
        }

        if (!inputBindingData.TryGetInputActionType(keyCode, out var inputActionType))
        {
            return;
        }

        actionRequester.RequestAction(inputActionType);
    }
}
