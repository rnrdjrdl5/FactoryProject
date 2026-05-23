using UnityEngine;

[Processor(typeof(EquipmentPopupProcessor))]
[Processor(typeof(EquipmentPopupInputLayerProcessor))]
public class EquipmentPopupProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetOrCreateContext<EquipmentPopupProcessorContext>();
    }
}
