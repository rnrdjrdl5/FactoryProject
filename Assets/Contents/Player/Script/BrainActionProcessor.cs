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

    public void Move(Vector2 direction)
    {
        if (moveAbility == null)
        {
            return;
        }

        moveAbility.Move(direction);
    }

    public void FollowTarget()
    {
        if (followAbility == null)
        {
            return;
        }

        followAbility.Move();
    }

    public void TryPick()
    {
        if (pickProcessor == null)
        {
            return;
        }

        pickProcessor.PickItem();
    }

    public bool TryUseSkill(string skillKey)
    {
        if (skillAbility == null)
        {
            return false;
        }

        return skillAbility.TryUseSkill(skillKey);
    }

    public bool TryUseUniqueSkill()
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
