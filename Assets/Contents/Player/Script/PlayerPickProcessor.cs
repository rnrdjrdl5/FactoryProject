using UnityEngine;

public class PlayerPickProcessor : Processor
{
    MainStorage mainStorage;
    TeamInventory teamInventory;
    
    public override void Ready()
    {
        base.Ready();

        mainStorage = FactoryEntry.MainStorage;
        teamInventory = mainStorage.GetEntityData<TeamInventory>();
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
        
        teamInventory.Inventory.AddItem(worldItem.ItemData.Key, 1);
        Realm.RemoveChild(worldItem);
    }
}
