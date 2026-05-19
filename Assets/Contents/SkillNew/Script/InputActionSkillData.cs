using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class InputActionSkillData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<KeyCode, string> skillKeyByKeyCode = new();

    public void Initialize(IInitData initData = null)
    {
        skillKeyByKeyCode.Clear();
    }

    public void Uninitialize()
    {
    }

    public void OnSetMessageBus()
    {
    }

    public void SetSkillKey(KeyCode keyCode, string skillKey)
    {
        skillKeyByKeyCode[keyCode] = skillKey ?? string.Empty;
    }

    public bool TryGetSkillKey(KeyCode keyCode, out string skillKey)
    {
        if (skillKeyByKeyCode.TryGetValue(keyCode, out skillKey) && !string.IsNullOrWhiteSpace(skillKey))
        {
            return true;
        }

        skillKey = string.Empty;
        return false;
    }

    public void ClearSkillKey(KeyCode keyCode)
    {
        skillKeyByKeyCode[keyCode] = string.Empty;
    }

    public IReadOnlyDictionary<KeyCode, string> GetAllSkillKeys()
    {
        return skillKeyByKeyCode;
    }
}
