using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class InputActionSkillData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<InputActionType, string> skillKeyByInputAction = new();

    public void Initialize(IInitData initData = null)
    {
        skillKeyByInputAction.Clear();

        foreach (InputActionType inputActionType in Enum.GetValues(typeof(InputActionType)))
        {
            skillKeyByInputAction[inputActionType] = string.Empty;
        }
    }

    public void Uninitialize()
    {
    }

    public void OnSetMessageBus()
    {
    }

    public void SetSkillKey(InputActionType inputActionType, string skillKey)
    {
        skillKeyByInputAction[inputActionType] = skillKey ?? string.Empty;
    }

    public bool TryGetSkillKey(InputActionType inputActionType, out string skillKey)
    {
        if (skillKeyByInputAction.TryGetValue(inputActionType, out skillKey) && !string.IsNullOrWhiteSpace(skillKey))
        {
            return true;
        }

        skillKey = string.Empty;
        return false;
    }

    public void ClearSkillKey(InputActionType inputActionType)
    {
        skillKeyByInputAction[inputActionType] = string.Empty;
    }

    public IReadOnlyDictionary<InputActionType, string> GetAllSkillKeys()
    {
        return skillKeyByInputAction;
    }
}
