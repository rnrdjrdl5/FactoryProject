public abstract class BasePopupLayerOwnerProcessor : Processor
{
    InputRealmProcessorAbility inputRealmProcessorAbility;

    protected abstract InputLayerType LayerType { get; }

    public override void Ready()
    {
        base.Ready();

        var mainRealm = Entity.GetParent<MainRealm>();
        var inputRealm = mainRealm?.GetChild<InputRealm>();
        inputRealmProcessorAbility = inputRealm?.GetAbility<InputRealmProcessorAbility>();
        inputRealmProcessorAbility?.PushLayer(LayerType);
    }

    public override void Uninitialize()
    {
        inputRealmProcessorAbility?.RemoveLayer(LayerType);
        inputRealmProcessorAbility = null;

        base.Uninitialize();
    }
}
