using UnityEngine;

[Processor(typeof(GlobalActionProcessor))]
[Processor(typeof(GlobalTeamProcessor))]
[Processor(typeof(PlayerInputLayerProcessor))]
[Processor(typeof(TeamInputLayerProcessor))]
[Processor(typeof(EquipmentInputLayerProcessor))]
[Processor(typeof(InventoryInputLayerProcessor))]
public class GlobalRealmProcessorAbility : ProcessorAbility
{
    protected override void CreateProcessorContext()
    {
        GetContext<GlobalRealmProcessorContext>();
    }
}
