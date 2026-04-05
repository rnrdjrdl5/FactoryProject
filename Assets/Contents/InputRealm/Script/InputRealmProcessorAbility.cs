using UnityEngine;

[Processor(typeof(GlobalInputCommandProcessor))]
[Processor(typeof(BrainInputCommandProcessor))]
[Processor(typeof(GameplayInputLayerProcessor))]
public class InputRealmProcessorAbility : FrameworkInputProcessorAbility
{
}
