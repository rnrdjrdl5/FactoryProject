using System;
using UnityEngine;

public class WorldItem : Entity
{
    public static string PrefabPath = $"Item/{nameof(WorldItem)}";
    public Item Item => item;

    Item item;
    string itemKey;

    protected override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        if (initData is ItemInitData itemInitData)
        {
            itemKey = itemInitData.ItemKey;
            item = Item.Create(itemKey, itemInitData.itemAmount);
            transform.position = itemInitData.Position;
        }
        
        base.Initialize(initData);
    }
}

public class ItemInitData : IInitData
{
    public string ItemKey;
    public Vector3 Position;
    public int itemAmount;
}
