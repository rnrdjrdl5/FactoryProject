public class PlayerEquipProcessor : Processor
{
    Equipment equipment;
    Stat stat;
    
    public override void Ready()
    {
        base.Ready();

        stat = Entity.GetEntityData<Stat>();
        equipment = Entity.GetEntityData<Equipment>();

        equipment.OnEquip += OnEquip;
        equipment.OnUnequip += OnUnequip;
    }

    public override void Uninitialize()
    {
        equipment.OnEquip -= OnEquip;
        equipment.OnUnequip -= OnUnequip;
        
        base.Uninitialize();
    }

    void OnEquip(Item item)
    {
        stat.AddStats(item.ItemData);
    }

    void OnUnequip(Item item)
    {
        stat.RemoveStats(item.ItemData);
    }
}