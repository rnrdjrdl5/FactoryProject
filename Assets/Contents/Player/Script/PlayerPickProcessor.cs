using UnityEngine;

public class PlayerPickProcessor : Processor
{
    MainStorage mainStorage;
    MainStorageProcessor mainStorageProcessor;
    
    public override void Ready()
    {
        base.Ready();

        mainStorage = FactoryEntry.MainStorage;
        var processorAbility = mainStorage.GetAbility<MainStorageProcessorAbility>();
        mainStorageProcessor = processorAbility.GetProcessor<MainStorageProcessor>();
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

        mainStorageProcessor.AddPlayerStorage(worldItem.Item);
        Realm.RemoveChild(worldItem);
    }
}
