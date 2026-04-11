public class BrainActionExecutionContext
{
    public Entity ControlledEntity { get; set; }
    public PlayerMoveAbility MoveAbility { get; set; }
    public PlayerFollowAbility FollowAbility { get; set; }
    public SkillAbility SkillAbility { get; set; }
    public PlayerPickProcessor PickProcessor { get; set; }
    public PlayerModelProcessor ModelProcessor { get; set; }

    public bool TryInitialize(IControlled controlled)
    {
        Reset();

        ControlledEntity = controlled as Entity;
        if (ControlledEntity == null)
        {
            return false;
        }

        MoveAbility = ControlledEntity.GetAbility<PlayerMoveAbility>();
        FollowAbility = ControlledEntity.GetAbility<PlayerFollowAbility>();
        SkillAbility = ControlledEntity.GetAbility<SkillAbility>();

        var processorAbility = ControlledEntity.GetAbility<PlayerProcessorAbility>();
        PickProcessor = processorAbility?.GetProcessor<PlayerPickProcessor>();
        ModelProcessor = processorAbility?.GetProcessor<PlayerModelProcessor>();
        return true;
    }

    public bool Matches(IControlled controlled)
    {
        return ControlledEntity == controlled;
    }

    public void Reset()
    {
        ControlledEntity = null;
        MoveAbility = null;
        FollowAbility = null;
        SkillAbility = null;
        PickProcessor = null;
        ModelProcessor = null;
    }
}
