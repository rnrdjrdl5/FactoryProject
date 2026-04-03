using UnityEngine;

[Processor(typeof(GlobalProcessor))]
[Processor(typeof(GlobalInputProcessor))]
[Processor(typeof(GlobalTeamProcessor))]
public class GlobalRealmProcessorAbility : ProcessorAbility
{
}
