public class BrainActionProcessor : Processor
{
    Brain brain;
    BrainActionExecutionContext executionContext;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        executionContext = new BrainActionExecutionContext();

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
        executionContext = null;

        base.Uninitialize();
    }

    public bool RequestAction<TAction>(TAction action)
        where TAction : struct, IBrainAction
    {
        return action.Execute(executionContext);
    }

    void RefreshControlledCache(IControlled controlled)
    {
        executionContext?.TryInitialize(controlled);
    }

    void ClearControlledCache(IControlled controlled)
    {
        if (executionContext == null || !executionContext.Matches(controlled))
        {
            return;
        }

        ResetControlledCache();
    }

    void ResetControlledCache()
    {
        executionContext?.Reset();
    }
}
