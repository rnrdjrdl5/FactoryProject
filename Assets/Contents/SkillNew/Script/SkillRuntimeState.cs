using UnityEngine;

public class SkillRuntimeState
{
    public string SkillKey { get; private set; }
    public float CurrentCooldown { get; private set; }

    public static SkillRuntimeState Create(string skillKey)
    {
        return new SkillRuntimeState
        {
            SkillKey = skillKey,
            CurrentCooldown = 0f
        };
    }

    public bool CanUse()
    {
        return CurrentCooldown <= 0f;
    }

    public void SetCooldown(float cooldown)
    {
        CurrentCooldown = Mathf.Max(0f, cooldown);
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (CurrentCooldown <= 0f)
        {
            return;
        }

        CurrentCooldown = Mathf.Max(0f, CurrentCooldown - deltaTime);
    }
}
