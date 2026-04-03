using UnityEngine;

[Processor(typeof(GlobalActionProcessor))]
[Processor(typeof(PhysicalInputProcessor))]
[Processor(typeof(GlobalTokenInputProcessor))]
[Processor(typeof(BrainTokenInputProcessor))]
[Processor(typeof(GlobalInputProcessor))]
[Processor(typeof(BrainInputProcessor))]
[Processor(typeof(GlobalTeamProcessor))]
public class GlobalRealmProcessorAbility : ProcessorAbility
{
}
