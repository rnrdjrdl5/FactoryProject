using UnityEngine;

[Processor(typeof(TeamPopupProcessor))]
[Processor(typeof(TeamPopupInputLayerProcessor))]
public class TeamPopupProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetOrCreateContext<TeamPopupProcessorContext>();
    }
}
