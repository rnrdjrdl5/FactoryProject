using UnityEngine;

[Processor(typeof(PhysicalTokenRouterProcessor))]
[Processor(typeof(GlobalInputCommandMapper))]
[Processor(typeof(BrainInputCommandMapper))]
[Processor(typeof(GlobalInputCommandProcessor))]
[Processor(typeof(BrainInputCommandProcessor))]
[Processor(typeof(GameplayInputLayerProcessor))]
[Processor(typeof(TeamInputLayerProcessor))]
[Processor(typeof(EquipmentInputLayerProcessor))]
[Processor(typeof(InventoryInputLayerProcessor))]
public class InputRealmProcessorAbility : LayerProcessorAbility<InputLayerType, PhysicalInputTokenEvent>
{
    public override void Ready()
    {
        base.Ready();

        PushLayer(InputLayerType.Gameplay);
    }

    public override void Uninitialize()
    {
        RemoveLayer(InputLayerType.Gameplay);

        base.Uninitialize();
    }
}
