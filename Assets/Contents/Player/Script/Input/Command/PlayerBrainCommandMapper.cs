using UnityEngine;

public class PlayerBrainCommandMapper : IInputCommandMapper<PlayerBrainCommand>
{
    readonly Entity entity;

    public PlayerBrainCommandMapper(Entity entity)
    {
        this.entity = entity;
    }

    public bool TryMap(InputContext input, out PlayerBrainCommand command)
    {
        if (input.KeyCode == KeyCode.None)
        {
            command = PlayerBrainCommand.CreateMove(input.Axis);
            return true;
        }

        if (input.StateType != InputStateType.Pressed)
        {
            command = default;
            return false;
        }

        if (input.KeyCode == KeyCode.Z)
        {
            command = PlayerBrainCommand.CreatePick();
            return true;
        }

        return TryMapSkillCommand(input.KeyCode, out command);
    }

    bool TryMapSkillCommand(KeyCode keyCode, out PlayerBrainCommand command)
    {
        var playerData = ResolvePlayerData();
        if (!InputActionSkillLogic.TryGetSkillKey(playerData, keyCode, out var skillKey))
        {
            command = default;
            return false;
        }

        command = PlayerBrainCommand.CreateUseSkill(skillKey);
        return true;
    }

    PlayerData ResolvePlayerData()
    {
        var controlledEntity = ResolvePlayerControlledEntity();
        return controlledEntity?.GetEntityData<PlayerData>();
    }

    Entity ResolvePlayerControlledEntity()
    {
        var mainRealm = entity as MainRealm ?? entity.GetParent<MainRealm>();
        if (mainRealm == null)
        {
            return null;
        }

        foreach (var brain in mainRealm.GetChildren<Brain>())
        {
            if (brain == null || brain.ControlMode != BrainControlMode.PlayerInput)
            {
                continue;
            }

            var context = brain.GetProcessorContext<BrainProcessorContext>();
            if (context?.BrainActionProcessor == null)
            {
                continue;
            }

            return brain.Controll as Entity;
        }

        return null;
    }
}
