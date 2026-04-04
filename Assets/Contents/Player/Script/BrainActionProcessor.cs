using UnityEngine;

public class BrainActionProcessor : Processor
{
    Brain brain;
    BrainActionExecutionContext executionContext;
    BrainInputActionResolver inputActionResolver;
    BrainIntentActionResolver intentActionResolver;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        executionContext = new BrainActionExecutionContext();
        inputActionResolver = new BrainInputActionResolver();
        intentActionResolver = new BrainIntentActionResolver();

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
        inputActionResolver = null;
        intentActionResolver = null;

        base.Uninitialize();
    }

    public void RequestAction(BrainActionRequest request)
    {
        switch (request.RequestType)
        {
            case BrainActionRequestType.Input:
                inputActionResolver?.TryResolve(request, executionContext);
                return;
            case BrainActionRequestType.Intent:
                intentActionResolver?.TryResolve(request, executionContext);
                return;
        }
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
