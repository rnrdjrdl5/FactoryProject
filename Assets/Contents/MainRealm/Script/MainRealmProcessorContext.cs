public class MainRealmProcessorContext : IProcessorContext
{
    public Entity Entity { get; private set; }
    public MainRealmTeamProcessor MainRealmTeamProcessor { get; private set; }
    public MainRealmPlayerEntityProcessor MainRealmPlayerEntityProcessor { get; private set; }
    public MainRealmFlowProcessor MainRealmFlowProcessor { get; private set; }
    public MainRealmProcessor MainRealmProcessor { get; private set; }

    public bool TryInitialize(Entity entity)
    {
        var ability = entity?.GetAbility<MainRealmProcessorAbility>();
        if (ability == null)
        {
            Reset();
            return false;
        }

        Entity = entity;
        MainRealmTeamProcessor = ability.GetProcessor<MainRealmTeamProcessor>();
        MainRealmPlayerEntityProcessor = ability.GetProcessor<MainRealmPlayerEntityProcessor>();
        MainRealmFlowProcessor = ability.GetProcessor<MainRealmFlowProcessor>();
        MainRealmProcessor = ability.GetProcessor<MainRealmProcessor>();

        return true;
    }

    public void Reset()
    {
        Entity = null;
        MainRealmTeamProcessor = null;
        MainRealmPlayerEntityProcessor = null;
        MainRealmFlowProcessor = null;
        MainRealmProcessor = null;
    }
}
