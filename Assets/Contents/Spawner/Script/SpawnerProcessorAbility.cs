using UnityEngine;

[Processor(typeof(SpawnerProcessor))]
public class SpawnerProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetContext<SpawnerProcessorContext>();
    }
}
