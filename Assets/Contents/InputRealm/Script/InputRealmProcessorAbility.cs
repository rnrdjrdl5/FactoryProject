using UnityEngine;

[Processor(typeof(PhysicalTokenRouterProcessor))]
[Processor(typeof(GlobalInputCommandMapper))]
[Processor(typeof(BrainInputCommandMapper))]
[Processor(typeof(GlobalInputCommandProcessor))]
[Processor(typeof(BrainInputCommandProcessor))]
public class InputRealmProcessorAbility : ProcessorAbility
{
}
