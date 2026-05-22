using UnityEngine;

public struct MoveBrainAction : IBrainAction
{
    readonly Vector2 direction;

    public MoveBrainAction(Vector2 direction = default)
    {
        this.direction = direction;
    }

    public bool Execute(BrainActionContext context)
    {
        var moveAbility = context?.ControlledEntity?.GetAbility<PlayerMoveAbility>();
        if (moveAbility == null)
        {
            return false;
        }

        var moveDelta = moveAbility.Move(direction);
        BrainActionUtility.RefreshMoveAnimation(context, moveDelta);
        return true;
    }
}

public struct FollowTargetBrainAction : IBrainAction
{
    public bool Execute(BrainActionContext context)
    {
        var followAbility = context?.ControlledEntity?.GetAbility<PlayerFollowAbility>();
        if (followAbility == null)
        {
            return false;
        }

        var moveDelta = followAbility.Move();
        BrainActionUtility.RefreshMoveAnimation(context, moveDelta);
        return true;
    }
}

public struct PickBrainAction : IBrainAction
{
    public bool Execute(BrainActionContext context)
    {
        var pickProcessor = context
            ?.ControlledEntity
            ?.GetProcessorContext<PlayerProcessorContext>()
            ?.PlayerPickProcessor;
        if (pickProcessor == null)
        {
            return false;
        }

        pickProcessor.PickItem();
        return true;
    }
}

public struct UseSkillBrainAction : IBrainAction
{
    readonly string skillKey;

    public UseSkillBrainAction(string skillKey)
    {
        this.skillKey = skillKey;
    }

    public bool Execute(BrainActionContext context)
    {
        var skillAbility = context?.ControlledEntity?.GetAbility<SkillAbility>();
        return skillAbility != null && skillAbility.TryUseSkill(skillKey);
    }
}

public struct UseMainAttackSkillBrainAction : IBrainAction
{
    public bool Execute(BrainActionContext context)
    {
        var playerData = context?.ControlledEntity?.GetEntityData<PlayerData>();
        if (!InputActionSkillLogic.TryGetMainAttackSkillKey(playerData, out var skillKey))
        {
            return false;
        }

        return new UseSkillBrainAction(skillKey).Execute(context);
    }
}

public static class BrainActionUtility
{
    public static void RefreshMoveAnimation(BrainActionContext context, Vector2 moveDelta)
    {
        var modelProcessor = context
            ?.ControlledEntity
            ?.GetProcessorContext<PlayerProcessorContext>()
            ?.PlayerModelProcessor;
        if (modelProcessor == null)
        {
            return;
        }

        var stateType = moveDelta.sqrMagnitude > 0f ? PixemAnimationType.Run : PixemAnimationType.Idle;
        if (moveDelta.x != 0f)
        {
            modelProcessor.SetFlip(moveDelta.x > 0f);
        }

        modelProcessor.SetStateType(stateType);
    }
}
