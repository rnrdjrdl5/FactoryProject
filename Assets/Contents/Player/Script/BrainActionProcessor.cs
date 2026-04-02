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
        switch (request)
        {
            case MoveActionRequest moveRequest:
                Move(moveRequest.Direction);
                break;

            case PickActionRequest:
                TryPick();
                break;

            case UseUniqueSkillActionRequest:
                TryUseUniqueSkill();
                break;

            case FollowTargetActionRequest:
                FollowTarget();
                break;
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

    bool TryUseUniqueSkill()
    {
        var player = controlledEntity as Player;
        if (player?.TableData == null)
        {
            return false;
        }

        return TryUseSkill(player.TableData.uniqueSkillKey);
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
