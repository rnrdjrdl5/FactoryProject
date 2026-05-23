using UnityEngine;

[Processor(typeof(MainRealmTeamProcessor))]
[Processor(typeof(MainRealmPlayerEntityProcessor))]
[Processor(typeof(MainRealmFlowProcessor))]
[Processor(typeof(MainRealmProcessor))]
public class MainRealmProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetContext<MainRealmProcessorContext>();
    }
}
