using UnityEngine;

public class InventoryInputLayerProcessor : BaseInputLayerProcessor
{
    GlobalActionProcessor globalActionProcessor;

    public override void Ready()
    {
        base.Ready();

        RefreshGlobalActionProcessor();
    }

    public override void Uninitialize()
    {
        globalActionProcessor = null;

        base.Uninitialize();
    }

    public override LayerResult ProcessInput(InputContext input)
    {
        if (input.StateType != InputStateType.Pressed || input.KeyCode != KeyCode.I)
        {
            return LayerResult.Pass;
        }

        RefreshGlobalActionProcessor();
        if (globalActionProcessor == null)
        {
            return LayerResult.Pass;
        }

        globalActionProcessor.OpenInventory();
        return LayerResult.Consume;
    }

    void RefreshGlobalActionProcessor()
    {
        if (globalActionProcessor != null)
        {
            return;
        }

        var globalRealm = Entity.GetFromRoot<GlobalRealm>();
        var processorAbility = globalRealm?.GetAbility<GlobalRealmProcessorAbility>();
        globalActionProcessor = processorAbility?.GetProcessor<GlobalActionProcessor>();
    }
}
