public class GameplayInputLayerProcessor : BaseInputLayerProcessor
{
    GlobalInputCommandMapper globalInputCommandMapper;
    BrainInputCommandMapper brainInputCommandMapper;

    public override InputLayerType LayerType => InputLayerType.Gameplay;

    public override void Ready()
    {
        base.Ready();

        var inputRealmProcessorAbility = Entity.GetAbility<InputRealmProcessorAbility>();
        globalInputCommandMapper = inputRealmProcessorAbility?.GetProcessor<GlobalInputCommandMapper>();
        brainInputCommandMapper = inputRealmProcessorAbility?.GetProcessor<BrainInputCommandMapper>();
    }

    public override void Uninitialize()
    {
        globalInputCommandMapper = null;
        brainInputCommandMapper = null;

        base.Uninitialize();
    }

    public override LayerResult ProcessInput(PhysicalInputTokenEvent input)
    {
        globalInputCommandMapper?.RequestTokenInput(input);
        brainInputCommandMapper?.RequestTokenInput(input);

        return LayerResult.Consume;
    }
}
