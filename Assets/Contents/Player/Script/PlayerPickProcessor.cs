using Tables;
using UnityEngine;

public class PlayerPickProcessor : Processor
{
    Bag bag;
    Inventory animalInventory; 
    
    public override void Ready()
    {
        base.Ready();

        bag = Entity.GetEntityData<Bag>();
        animalInventory = bag.GetInventory(ItemType.Animal);
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
        
        animalInventory.AddItem(worldItem.ItemData.Key, 1);
        Realm.RemoveChild(worldItem);
    }
}
