using UnityEngine;

[Processor(typeof(BrainFlowProcessor))]
[Processor(typeof(BrainProcessor))]
[Processor(typeof(BrainActionProcessor))]
public class BrainProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetOrCreateContext<BrainProcessorContext>();
    }
}
