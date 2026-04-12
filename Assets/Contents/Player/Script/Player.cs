using System;
using System.Linq;
using Tables;
using UnityEngine;

[EntityData(typeof(PlayerData))]
public class Player : Entity
{
    public Tables.Player TableData => tableData;
    public PlayerView View => view;

    [SerializeField] PlayerView view;
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            var top = Item.Create(Tables.TablesKey.Item_Top_Top_1, 1);
            var mainStorage = GetFromRoot<MainStorage>();
            var bag = mainStorage.GetEntityData<Bag>();
            var topInventory = bag.GetInventory(ItemSlotType.Top);
            topInventory.AddItem(top);
            
            var item = topInventory.Items.FirstOrDefault();
            if (item != null)
            {
                playerData.Equipment.TryEquipItem(item);
            }
        }
    }
}

public class PlayerInitData : IInitData , IUniqueId, IPositionData
{
    public long UniqueId { get; set; }
    public string PlayerKey;
    public PlayerOriginType OriginType;
    public Vector3 Position { get; set; }
}
