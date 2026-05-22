using UnityEngine;

[Processor(typeof(WorldItemEquipProcessor))]
public class WorldItemProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetOrCreateContext<WorldItemProcessorContext>();
    }
}
