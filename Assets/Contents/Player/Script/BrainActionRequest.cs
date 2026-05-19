using UnityEngine;

public interface IBrainAction
{
    bool Execute(BrainActionExecutionContext context);
}

public struct MoveBrainAction : IBrainAction
{
    readonly Vector2 direction;

    public MoveBrainAction(Vector2 direction = default)
    {
        this.direction = direction;
    }

    public bool Execute(BrainActionExecutionContext context)
    {
        if (context?.MoveAbility == null)
        {
            return false;
        }

        var moveDelta = context.MoveAbility.Move(direction);
        BrainActionUtility.RefreshMoveAnimation(context, moveDelta);
        return true;
    }
}

public struct FollowTargetBrainAction : IBrainAction
{
    public bool Execute(BrainActionExecutionContext context)
    {
        if (context?.FollowAbility == null)
        {
            return false;
        }

        var moveDelta = context.FollowAbility.Move();
        BrainActionUtility.RefreshMoveAnimation(context, moveDelta);
        return true;
    }
}

public struct PickBrainAction : IBrainAction
{
    public bool Execute(BrainActionExecutionContext context)
    {
        if (context?.PickProcessor == null)
        {
            return false;
        }

        context.PickProcessor.PickItem();
        return true;
    }
}

public struct UseSkillBrainAction : IBrainAction
{
    readonly KeyCode keyCode;

    public UseSkillBrainAction(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }

    public bool Execute(BrainActionExecutionContext context)
    {
        var playerData = context?.ControlledEntity?.GetEntityData<PlayerData>();
        if (!InputActionSkillLogic.TryGetSkillKey(playerData, keyCode, out var skillKey))
        {
            return false;
        }

        return context.SkillAbility != null && context.SkillAbility.TryUseSkill(skillKey);
    }
}

public struct UseMainAttackSkillBrainAction : IBrainAction
{
    public bool Execute(BrainActionExecutionContext context)
    {
        return new UseSkillBrainAction(KeyCode.Mouse0).Execute(context);
    }
}

public static class BrainActionUtility
{
    public static void RefreshMoveAnimation(BrainActionExecutionContext context, Vector2 moveDelta)
    {
        if (context?.ModelProcessor == null)
        {
            return;
        }

        var stateType = moveDelta.sqrMagnitude > 0f ? PixemAnimationType.Run : PixemAnimationType.Idle;
        if (moveDelta.x != 0f)
        {
            context.ModelProcessor.SetFlip(moveDelta.x > 0f);
        }

        context.ModelProcessor.SetStateType(stateType);
    }
}
