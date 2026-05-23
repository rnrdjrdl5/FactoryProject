using UnityEngine;

[Processor(typeof(WorldItemEquipProcessor))]
public class WorldItemProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetContext<WorldItemProcessorContext>();
    }
}
