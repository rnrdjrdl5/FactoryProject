using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GlobalInputBindingData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<KeyCode, GlobalInputActionType> inputActionTypeByKeyCode = new();

    public void Initialize(IInitData initData = null)
    {
        inputActionTypeByKeyCode.Clear();

        SetInputActionType(KeyCode.F1, GlobalInputActionType.OpenTeam);
        SetInputActionType(KeyCode.F2, GlobalInputActionType.OpenEquipment);
        SetInputActionType(KeyCode.I, GlobalInputActionType.OpenInventory);
    }

    public void Uninitialize()
    {
        inputActionTypeByKeyCode.Clear();
    }

    public void OnSetMessageBus()
    {
    }

    public void SetInputActionType(KeyCode keyCode, GlobalInputActionType inputActionType)
    {
        inputActionTypeByKeyCode[keyCode] = inputActionType;
    }

    public bool TryGetInputActionType(KeyCode keyCode, out GlobalInputActionType inputActionType)
    {
        return inputActionTypeByKeyCode.TryGetValue(keyCode, out inputActionType);
    }
}
