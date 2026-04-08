using System;
using UnityEngine;

[EntityData(typeof(PlayerData))]
public class Player : Entity
{
    public Tables.Player TableData => tableData;
    
    Tables.Player tableData;
    string playerKey;
    PlayerData playerData;

    protected override void PreInitialize(IInitData initData = null)
    {
        base.PreInitialize(initData);
        
        var mainStorage = GetFromRoot<MainStorage>();
        var playerStorage = mainStorage.GetEntityData<PlayerStorage>();
        if (playerStorage.PlayerDataByKey.TryGetValue(UniqueId, out playerData))
        {
            AddOverrideEntityData(playerData);
        }
    }

    protected override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        if (initData is PlayerInitData playerInitData)
        {
            playerKey = playerInitData.PlayerKey;
            tableData = Tables.Player.Get(playerKey);
        }
        
        base.Initialize(initData);

        playerData = GetEntityData<PlayerData>();
        playerData.UniqueId = UniqueId;
    }
}

public class PlayerInitData : IInitData , IUniqueId, IPositionData
{
    public long UniqueId { get; set; }
    public string PlayerKey;
    public PlayerOriginType OriginType;
    public Vector3 Position { get; set; }
}
