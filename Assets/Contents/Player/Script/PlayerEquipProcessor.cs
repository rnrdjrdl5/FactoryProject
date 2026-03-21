public class PlayerEquipProcessor : Processor
{
    Equipment equipment;
    Stat stat;
    PlayerData playerData;
    
    public override void Ready()
    {
        base.Ready();

        playerData = Entity.GetEntityData<PlayerData>();
        stat = playerData?.Stat;
        equipment = playerData?.Equipment;

        if (equipment?.MessageBus != null)
        {
            equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }
    }

    public override void Uninitialize()
    {
        if (equipment?.MessageBus != null)
        {
            equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
            equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        }
        
        base.Uninitialize();
    }

    void EquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        stat.AddStats(msg.Item.ItemData);
    }

    void UnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        stat.RemoveStats(msg.Item.ItemData);
    }
}
