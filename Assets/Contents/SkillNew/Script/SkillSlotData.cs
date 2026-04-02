using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SkillSlotData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<SkillSlotType, string> skillKeyBySlot = new();

    public void Initialize(IInitData initData = null)
    {
        skillKeyBySlot.Clear();

        foreach (SkillSlotType slotType in Enum.GetValues(typeof(SkillSlotType)))
        {
            skillKeyBySlot[slotType] = string.Empty;
        }
    }

    public void Uninitialize()
    {
    }

    public void OnSetMessageBus()
    {
    }

    public void SetSkillKey(SkillSlotType slotType, string skillKey)
    {
        skillKeyBySlot[slotType] = skillKey ?? string.Empty;
    }

    public bool TryGetSkillKey(SkillSlotType slotType, out string skillKey)
    {
        if (skillKeyBySlot.TryGetValue(slotType, out skillKey) && !string.IsNullOrWhiteSpace(skillKey))
        {
            return true;
        }

        skillKey = string.Empty;
        return false;
    }

    public void ClearSkillKey(SkillSlotType slotType)
    {
        skillKeyBySlot[slotType] = string.Empty;
    }

    public IReadOnlyDictionary<SkillSlotType, string> GetAllSkillKeys()
    {
        return skillKeyBySlot;
    }
}
