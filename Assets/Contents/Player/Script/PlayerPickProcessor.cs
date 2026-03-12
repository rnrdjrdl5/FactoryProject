using Tables;
using UnityEngine;

public class PlayerPickProcessor : Processor
{
    Bag bag;
    
    public void PickItem(WorldItem worldItem)
    {
        bag ??= Entity.GetEntityData<Bag>();
        var inventory = bag.GetInventory(ItemType.Animal);
        inventory.AddItem(worldItem.ItemData.Key, 1);
        
        Realm.RemoveChild(worldItem);
    }
}

public class PlayerPickMessage
{
    public class PickMessage
    {
        
    }
}
