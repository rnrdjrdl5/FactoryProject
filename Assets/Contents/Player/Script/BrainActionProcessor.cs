using UnityEngine;

public class BrainActionProcessor : Processor, IBrainActionRequester
{
    Brain brain;
    Entity controlledEntity;
    IBrainActionRequestSource actionRequestSource;
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

        actionRequestSource = Entity.GetAbility<BrainInputAbility>();
        actionRequestSource?.SetActionRequester(this);
    }

    public override void Uninitialize()
    {
        actionRequestSource?.SetActionRequester(null);
        actionRequestSource = null;

        if (brain != null)
        {
            brain.OnAttachControll -= RefreshControlledCache;
            brain.OnDetachControll -= ClearControlledCache;
        }

        ResetControlledCache();

        base.Uninitialize();
    }

    public void RequestAction(IBrainActionRequest request)
    {
        if (TryResolveActionRequest(request))
        {
            return;
        }

        TryExecuteActionRequest(request);
    }

    bool TryResolveActionRequest(IBrainActionRequest request)
    {
        switch (request)
        {
            case PerformInputActionRequest inputActionRequest:
                return TryResolveInputAction(inputActionRequest.InputActionType);

            case PerformIntentActionRequest intentActionRequest:
                return TryResolveIntentAction(intentActionRequest.IntentActionType);

            default:
                return false;
        }
    }

    bool TryExecuteActionRequest(IBrainActionRequest request)
    {
        switch (request)
        {
            case PerformCustomActionRequest customActionRequest:
                return TryPerformCustomAction(customActionRequest);

            case PerformSystemActionRequest systemActionRequest:
                return TryPerformSystemAction(systemActionRequest.SystemActionType);

            case UseSkillRequest useSkillRequest:
                return TryUseSkill(useSkillRequest.SkillKey);

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

    bool TryResolveInputAction(InputActionType inputActionType)
    {
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
                return TryPerformCustomAction(new PerformCustomActionRequest
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

    bool TryPerformCustomAction(PerformCustomActionRequest customActionRequest)
    {
        switch (customActionRequest.CustomActionType)
        {
            case CustomActionType.Move:
                Move(customActionRequest.Direction);
                return true;
            case CustomActionType.FollowTarget:
                FollowTarget();
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
