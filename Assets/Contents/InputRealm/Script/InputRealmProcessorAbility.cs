using UnityEngine;

[Processor(typeof(PhysicalTokenRouterProcessor))]
[Processor(typeof(GlobalInputCommandMapper))]
[Processor(typeof(BrainInputCommandMapper))]
[Processor(typeof(GlobalInputCommandProcessor))]
[Processor(typeof(BrainInputCommandProcessor))]
[Processor(typeof(GameplayInputLayerProcessor))]
public class InputRealmProcessorAbility : LayerProcessorAbility<PhysicalInputTokenEvent>
{
}
