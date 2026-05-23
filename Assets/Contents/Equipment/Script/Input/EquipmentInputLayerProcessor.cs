using UnityEngine;

public class EquipmentInputLayerProcessor : BaseInputLayerProcessor
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
        if (input.StateType != InputStateType.Pressed || input.KeyCode != KeyCode.F2)
        {
            return LayerResult.Pass;
        }

        RefreshGlobalActionProcessor();
        if (globalActionProcessor == null)
        {
            return LayerResult.Pass;
        }

        globalActionProcessor.OpenEquipment();
        return LayerResult.Consume;
    }

    void RefreshGlobalActionProcessor()
    {
        if (globalActionProcessor != null)
        {
            return;
        }

        var globalRealm = Entity.GetFromRoot<GlobalRealm>();
        globalActionProcessor = globalRealm
            ?.GetProcessorContext<GlobalRealmProcessorContext>()
            ?.GlobalActionProcessor;
    }
}
