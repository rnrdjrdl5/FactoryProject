public class BrainProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public BrainFlowProcessor BrainFlowProcessor { get; private set; }
    public BrainProcessor BrainProcessor { get; private set; }
    public BrainActionProcessor BrainActionProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<BrainProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        BrainFlowProcessor = ability.GetProcessor<BrainFlowProcessor>();
        BrainProcessor = ability.GetProcessor<BrainProcessor>();
        BrainActionProcessor = ability.GetProcessor<BrainActionProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        BrainFlowProcessor = null;
        BrainProcessor = null;
        BrainActionProcessor = null;
    }
}
