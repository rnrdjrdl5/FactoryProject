public class PlayerEquipProcessor : Processor
{
    Equipment equipment;
    Stat stat;
    
    public override void Ready()
    {
        base.Ready();

        stat = Entity.GetEntityData<Stat>();
        equipment = Entity.GetEntityData<Equipment>();

        equipment.MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
        equipment.MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
    }

    public override void Uninitialize()
    {
        equipment.MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(EquipmentEquip);
        equipment.MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(UnequipmentEquip);
        
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