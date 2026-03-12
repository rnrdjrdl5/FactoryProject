using System;
using UnityEngine;

public class WorldItem : Entity
{
    public static string PrefabPath = $"Item/{nameof(WorldItem)}"; 
    
    public Tables.Item ItemData => itemData;
    
    Tables.Item itemData;
    string itemKey;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        if (initData is ItemInitData itemInitData)
        {
            itemKey = itemInitData.ItemKey;
            itemData = Tables.Item.Get(itemKey);
            transform.position = itemInitData.Position;
        }
        
        base.Initialize(initData);
    }
}

public class ItemInitData : IInitData
{
    public string ItemKey;
    public Vector3 Position;
}
