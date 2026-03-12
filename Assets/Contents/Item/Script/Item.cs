// ItemType 과 ItemData 테이블을 필요로 한다

using UnityEngine;

public class Item
{
    public Tables.Item ItemData => itemData;
    public long UniqueId => uniqueId;
    public string ItemKey => itemKey;
    public int MaxAmount => itemData.maxAmount;
    
    Tables.Item itemData;
    long uniqueId;
    string itemKey;
    int amount;

    public Item(string itemKey, int amount)
    {
        this.itemKey = itemKey;
        this.amount = amount;

        itemData = Tables.Item.Get(itemKey);
        uniqueId = IDLogic.NewUniqueId();
    }

    public void Acquire(int acquireCount)
    {
        amount = Mathf.Clamp(amount + acquireCount, 0, MaxAmount);
    }

    public void Spend(int spendCount)
    {
        amount = Mathf.Clamp(amount - spendCount, 0, MaxAmount);
    }

    public bool IsFull()
    {
        return amount == itemData.maxAmount;
    }

    public bool IsEmpty()
    {
        return amount <= 0;
    }

    public static Item Create(string itemKey, int amount)
    {
        var itemData = Tables.Item.Get(itemKey);
        return new Item(itemKey,amount);
    }
}