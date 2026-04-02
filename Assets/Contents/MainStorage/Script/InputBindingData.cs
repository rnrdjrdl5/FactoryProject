using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class InputBindingData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<KeyCode, InputActionType> inputActionTypeByKeyCode = new();

    public void Initialize(IInitData initData = null)
    {
        inputActionTypeByKeyCode.Clear();

        SetInputActionType(KeyCode.Z, InputActionType.Pick);
        SetInputActionType(KeyCode.Mouse0, InputActionType.MainAttack);
        SetInputActionType(KeyCode.Mouse1, InputActionType.SubAttack);
        SetInputActionType(KeyCode.Q, InputActionType.Skill1);
        SetInputActionType(KeyCode.E, InputActionType.Skill2);
        SetInputActionType(KeyCode.R, InputActionType.Skill3);
        SetInputActionType(KeyCode.Space, InputActionType.MainUtility);
        SetInputActionType(KeyCode.LeftShift, InputActionType.SubUtility);
    }

    public void Uninitialize()
    {
        inputActionTypeByKeyCode.Clear();
    }

    public void OnSetMessageBus()
    {
    }

    public void SetInputActionType(KeyCode keyCode, InputActionType inputActionType)
    {
        inputActionTypeByKeyCode[keyCode] = inputActionType;
    }

    public bool TryGetInputActionType(KeyCode keyCode, out InputActionType inputActionType)
    {
        return inputActionTypeByKeyCode.TryGetValue(keyCode, out inputActionType);
    }
}
