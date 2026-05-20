using UnityEngine;

public enum PlayerBrainCommandType
{
    None,
    Move,
    Pick,
    UseSkill,
}

public readonly struct PlayerBrainCommand
{
    public readonly PlayerBrainCommandType Type;
    public readonly Vector2 Axis;
    public readonly string SkillKey;

    PlayerBrainCommand(PlayerBrainCommandType type, Vector2 axis = default, string skillKey = null)
    {
        Type = type;
        Axis = axis;
        SkillKey = skillKey;
    }

    public static PlayerBrainCommand CreateMove(Vector2 axis)
    {
        return new PlayerBrainCommand(PlayerBrainCommandType.Move, axis);
    }

    public static PlayerBrainCommand CreatePick()
    {
        return new PlayerBrainCommand(PlayerBrainCommandType.Pick);
    }

    public static PlayerBrainCommand CreateUseSkill(string skillKey)
    {
        return new PlayerBrainCommand(PlayerBrainCommandType.UseSkill, skillKey: skillKey);
    }
}
