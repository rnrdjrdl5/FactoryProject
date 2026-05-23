using UnityEngine;

[Processor(typeof(InventoryProcessor))]
[Processor(typeof(InventoryPopupInputLayerProcessor))]
public class InventoryProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetOrCreateContext<InventoryProcessorContext>();
    }
}
