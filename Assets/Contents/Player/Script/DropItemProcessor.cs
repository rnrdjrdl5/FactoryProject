using UnityEngine;

public class DropItemProcessor : Processor
{
    public void TryDropItem(Vector3 position, float percent, string itemKey)
    {
        var result = MathUtils.Roll(MathUtils.MinRange, MathUtils.MaxRange, percent);
        if (!result)
        {
            return;
        }
        
        Realm.AddEntity<WorldItem>(WorldItem.PrefabPath, new ItemInitData{ItemKey = itemKey, Position = position, itemAmount = 1});
    }
}