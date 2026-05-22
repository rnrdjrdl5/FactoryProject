public class TeamPopupProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public TeamPopupProcessor TeamPopupProcessor { get; private set; }
    public TeamPopupInputLayerProcessor TeamPopupInputLayerProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<TeamPopupProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        TeamPopupProcessor = ability.GetProcessor<TeamPopupProcessor>();
        TeamPopupInputLayerProcessor = ability.GetProcessor<TeamPopupInputLayerProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        TeamPopupProcessor = null;
        TeamPopupInputLayerProcessor = null;
    }
}
