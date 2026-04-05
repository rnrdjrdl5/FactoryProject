public class GameplayInputLayerProcessor : BaseInputLayerProcessor
{
    GlobalInputCommandProcessor globalInputCommandProcessor;
    BrainInputCommandProcessor brainInputCommandProcessor;

    public override void Ready()
    {
        base.Ready();

        var inputRealmProcessorAbility = Entity.GetAbility<InputRealmProcessorAbility>();
        globalInputCommandProcessor = inputRealmProcessorAbility?.GetProcessor<GlobalInputCommandProcessor>();
        brainInputCommandProcessor = inputRealmProcessorAbility?.GetProcessor<BrainInputCommandProcessor>();
    }

    public override void Uninitialize()
    {
        globalInputCommandProcessor = null;
        brainInputCommandProcessor = null;

        base.Uninitialize();
    }

    public override LayerResult ProcessInput(PhysicalInputTokenEvent input)
    {
        if (GlobalInputCommandLogic.TryGetInputActionType(input.TokenType, out var globalInputActionType))
        {
            globalInputCommandProcessor?.RequestAction(globalInputActionType);
        }

        if (BrainInputCommandLogic.TryGetInputActionType(input.TokenType, out var inputActionType))
        {
            brainInputCommandProcessor?.RequestAction(inputActionType);
        }

        return LayerResult.Consume;
    }
}
