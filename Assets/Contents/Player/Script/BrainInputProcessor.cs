public class BrainInputProcessor : Processor
{
    BrainInputAbility brainInputAbility;

    public override void Ready()
    {
        base.Ready();

        brainInputAbility = Entity.GetAbility<BrainInputAbility>();
        if (brainInputAbility == null)
        {
            return;
        }

        var mainRealm = Entity.GetParent<MainRealm>();
        var mainStorage = mainRealm?.GetChild<MainStorage>();
        var inputBindingData = mainStorage?.GetEntityData<InputBindingData>();

        brainInputAbility.SetInputBindingData(inputBindingData);
    }
}
