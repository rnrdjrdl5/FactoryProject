using UnityEngine;

[Processor(typeof(PhysicalTokenRouterProcessor))]
[Processor(typeof(GlobalInputActionMapper))]
[Processor(typeof(BrainInputActionMapper))]
[Processor(typeof(GlobalInputProcessor))]
[Processor(typeof(BrainInputProcessor))]
public class InputRealmProcessorAbility : ProcessorAbility
{
}
