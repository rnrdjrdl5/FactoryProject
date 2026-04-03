public class BrainInputProcessor : Processor
{
    Brain brain;
    BrainInputAbility brainInputAbility;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        brain = Entity as Brain;
    }

    public override void Ready()
    {
        base.Ready();

        brainInputAbility = Entity.GetAbility<BrainInputAbility>();
        if (brainInputAbility == null)
        {
            return;
        }

        if (brain != null)
        {
            brain.OnAttachControll += RefreshInputBindingData;
            brain.OnDetachControll += ClearInputBindingData;
        }

        RefreshInputBindingData(brain?.Controll);
    }

    public override void Uninitialize()
    {
        if (brain != null)
        {
            brain.OnAttachControll -= RefreshInputBindingData;
            brain.OnDetachControll -= ClearInputBindingData;
        }

        brainInputAbility?.SetInputBindingData(null);

        base.Uninitialize();
    }

    void RefreshInputBindingData(IControlled controlled)
    {
        var playerEntity = controlled as Entity;
        var playerData = playerEntity?.GetEntityData<PlayerData>();
        brainInputAbility?.SetInputBindingData(playerData?.InputBindingData);
    }

    void ClearInputBindingData(IControlled controlled)
    {
        brainInputAbility?.SetInputBindingData(null);
    }
}
