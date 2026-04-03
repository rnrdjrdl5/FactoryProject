using UnityEngine;

public class BrainActionProcessor : Processor
{
    Brain brain;
    Entity controlledEntity;
    PlayerMoveAbility moveAbility;
    PlayerFollowAbility followAbility;
    SkillAbility skillAbility;
    PlayerPickProcessor pickProcessor;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        brain = Entity as Brain;
        if (brain == null)
        {
            return;
        }

        brain.OnAttachControll += RefreshControlledCache;
        brain.OnDetachControll += ClearControlledCache;

        RefreshControlledCache(brain.Controll);
    }

    public override void Ready()
    {
        base.Ready();
    }

    public override void Uninitialize()
    {
        if (brain != null)
        {
            brain.OnAttachControll -= RefreshControlledCache;
            brain.OnDetachControll -= ClearControlledCache;
        }

        ResetControlledCache();

        base.Uninitialize();
    }

    public void RequestAction(BrainActionRequest request)
    {
        switch (request.RequestType)
        {
            case BrainActionRequestType.Input:
                TryResolveInputAction(request);
                return;
            case BrainActionRequestType.Intent:
                TryResolveIntentAction(request.IntentActionType);
                return;
        }
    }

    bool TryExecuteActionRequest(PerformCustomActionRequest request)
    {
        switch (request.CustomActionType)
        {
            case CustomActionType.Move:
                Move(request.Direction);
                return true;
            case CustomActionType.FollowTarget:
                FollowTarget();
                return true;
            default:
                return false;
        }
    }

    void Move(Vector2 direction)
    {
        if (moveAbility == null)
        {
            return;
        }

        moveAbility.Move(direction);
    }

    void FollowTarget()
    {
        if (followAbility == null)
        {
            return;
        }

        followAbility.Move();
    }

    void TryPick()
    {
        if (pickProcessor == null)
        {
            return;
        }

        pickProcessor.PickItem();
    }

    bool TryUseSkill(string skillKey)
    {
        if (skillAbility == null)
        {
            return false;
        }

        return skillAbility.TryUseSkill(skillKey);
    }

    bool TryResolveInputAction(BrainActionRequest inputActionRequest)
    {
        if (inputActionRequest.InputActionType == InputActionType.Move)
        {
            return TryExecuteActionRequest(new PerformCustomActionRequest
            {
                CustomActionType = CustomActionType.Move,
                Direction = inputActionRequest.Direction
            });
        }

        var inputActionType = inputActionRequest.InputActionType;
        if (InputActionSystemLogic.TryGetSystemActionType(inputActionType, out var systemActionType))
        {
            return TryPerformSystemAction(systemActionType);
        }

        return TryRequestSkillFromInputAction(inputActionType);
    }

    bool TryResolveIntentAction(IntentActionType intentActionType)
    {
        switch (intentActionType)
        {
            case IntentActionType.FollowTarget:
                return TryExecuteActionRequest(new PerformCustomActionRequest
                {
                    CustomActionType = CustomActionType.FollowTarget
                });
            case IntentActionType.UseMainAttackSkill:
                return TryRequestSkillFromInputAction(InputActionType.MainAttack);
            default:
                return false;
        }
    }

    bool TryPerformSystemAction(SystemActionType systemActionType)
    {
        switch (systemActionType)
        {
            case SystemActionType.Pick:
                TryPick();
                return true;
            default:
                return false;
        }
    }

    bool TryRequestSkillFromInputAction(InputActionType inputActionType)
    {
        var playerData = controlledEntity?.GetEntityData<PlayerData>();
        if (!InputActionSkillLogic.TryGetSkillKey(playerData, inputActionType, out var skillKey))
        {
            return false;
        }

        return TryUseSkill(skillKey);
    }

    void RefreshControlledCache(IControlled controlled)
    {
        ResetControlledCache();

        controlledEntity = controlled as Entity;
        if (controlledEntity == null)
        {
            return;
        }

        moveAbility = controlledEntity.GetAbility<PlayerMoveAbility>();
        followAbility = controlledEntity.GetAbility<PlayerFollowAbility>();
        skillAbility = controlledEntity.GetAbility<SkillAbility>();

        var processorAbility = controlledEntity.GetAbility<PlayerProcessorAbility>();
        pickProcessor = processorAbility?.GetProcessor<PlayerPickProcessor>();
    }

    void ClearControlledCache(IControlled controlled)
    {
        if (controlledEntity != controlled)
        {
            return;
        }

        ResetControlledCache();
    }

    void ResetControlledCache()
    {
        controlledEntity = null;
        moveAbility = null;
        followAbility = null;
        skillAbility = null;
        pickProcessor = null;
    }
}
