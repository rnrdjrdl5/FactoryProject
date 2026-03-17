using UnityEngine;

public class PlayerPickProcessor : Processor
{
    MainStorage mainStorage;
    Bag bag;
    Inventory inventory; 
    
    public override void Ready()
    {
        base.Ready();

        mainStorage = FactoryEntry.MainStorage;
        bag = mainStorage.GetEntityData<Bag>();
        inventory = bag.GetInventory(Tables.ItemType.Player);
    }

    public void PickItem()
    {
        var worldItem = (WorldItem)null;
        var colliders = Physics2D.OverlapCircleAll(Entity.transform.position, 1.0f, Settings.LayerId.ItemMask);
        foreach (var collider in colliders)
        {
            var item = collider.GetComponent<WorldItem>();
            if (item != null)
            {
                worldItem = item;
                break;
            }
        }

        if (worldItem == null)
        {
            return;
        }
        
        inventory.AddItem(worldItem.ItemData.Key, 1);
        Realm.RemoveChild(worldItem);
    }
}
