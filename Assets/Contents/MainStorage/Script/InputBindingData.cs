using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class InputBindingData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<KeyCode, SkillSlotType> skillSlotTypeByKeyCode = new();

    public void Initialize(IInitData initData = null)
    {
        skillSlotTypeByKeyCode.Clear();

        SetSkillSlotType(KeyCode.Mouse0, SkillSlotType.MainAttack);
        SetSkillSlotType(KeyCode.Mouse1, SkillSlotType.SubAttack);
        SetSkillSlotType(KeyCode.Q, SkillSlotType.Skill1);
        SetSkillSlotType(KeyCode.E, SkillSlotType.Skill2);
        SetSkillSlotType(KeyCode.R, SkillSlotType.Skill3);
        SetSkillSlotType(KeyCode.Space, SkillSlotType.MainUtility);
        SetSkillSlotType(KeyCode.LeftShift, SkillSlotType.SubUtility);
    }

    public void Uninitialize()
    {
        skillSlotTypeByKeyCode.Clear();
    }

    public void OnSetMessageBus()
    {
    }

    public void SetSkillSlotType(KeyCode keyCode, SkillSlotType skillSlotType)
    {
        skillSlotTypeByKeyCode[keyCode] = skillSlotType;
    }

    public bool TryGetSkillSlotType(KeyCode keyCode, out SkillSlotType skillSlotType)
    {
        return skillSlotTypeByKeyCode.TryGetValue(keyCode, out skillSlotType);
    }
}
