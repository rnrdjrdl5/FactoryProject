using System.Collections.Generic;
using Newtonsoft.Json;

// 플레이어 데이터 + 아이템 통합 Storage
public class PlayerStorage : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<long, PlayerData> playerDataByKey = new();
    [JsonProperty] Dictionary<long, long> itemIdToPlayerId = new();
    [JsonProperty] Dictionary<long, long> equipItemIdToPlayerId = new();
    [JsonProperty] Inventory inventory;

    public IReadOnlyDictionary<long, PlayerData> PlayerDataByKey => playerDataByKey;
    public IReadOnlyDictionary<long, long> ItemIdToPlayerId => itemIdToPlayerId;
    public IReadOnlyDictionary<long, long> EquipItemIdToPlayerId => equipItemIdToPlayerId;
    public IReadOnlyList<Item> PlayerItem => inventory?.Items;
    public Inventory PlayerInventory => inventory;
    

    public void Initialize(IInitData initData = null)
    {
        playerDataByKey.Clear();
        itemIdToPlayerId.Clear();
        equipItemIdToPlayerId.Clear();

        if (inventory == null)
        {
            inventory = Inventory.Create(Tables.ItemSlotType.Player);
        }
        else
        {
            inventory.Initialize(Tables.ItemSlotType.Player);
        }
    }

    public void Uninitialize()
    {
        foreach (var playerData in playerDataByKey.Values)
        {
            playerData.Uninitialize();
        }

        playerDataByKey.Clear();
        itemIdToPlayerId.Clear();
        equipItemIdToPlayerId.Clear();
        inventory?.Uninitialize();
    }

    public void OnSetMessageBus()
    {
        if (MessageBus == null)
        {
            return;
        }

        if (inventory != null)
        {
            inventory.MessageBus = MessageBus;
            inventory.OnSetMessageBus();
        }

        foreach (var playerData in playerDataByKey.Values)
        {
            playerData.MessageBus = MessageBus;
            playerData.OnSetMessageBus();
        }
    }

    public Item AddItem(Item item)
    {
        return inventory.AddItem(item);
    }

    public bool TryRemoveItem(string itemKey, int amount)
    {
        return inventory.TryRemoveItem(itemKey, amount);
    }

    public void CreateAndAddPlayerData(long playerUid, string playerKey)
    {
        playerDataByKey.TryAdd(playerUid, PlayerData.Create(MessageBus, playerKey, playerUid, PlayerOriginType.PlayerOwned));
    }

    public bool RemovePlayerData(long playerKey)
    {
        if (!playerDataByKey.TryGetValue(playerKey, out var playerData))
        {
            return false;
        }

        playerData.Uninitialize();
        return playerDataByKey.Remove(playerKey);
    }

    public bool TryGetPlayerIdByItemId(long itemUid, out long playerId)
    {
        playerId = 0;
        return itemIdToPlayerId.TryGetValue(itemUid, out playerId);
    }

    public void AddItemToPlayerId(long itemUid, long playerUid)
    {
        itemIdToPlayerId.TryAdd(itemUid, playerUid);
    }

    public void RemoveFromItemUid(long itemUid)
    {
        itemIdToPlayerId.Remove(itemUid);
    }

    public bool TryGetPlayerIdByEquipItemId(long itemUid, out long playerId)
    {
        return equipItemIdToPlayerId.TryGetValue(itemUid, out playerId);
    }

    public void AddEquipItemToPlayerId(long itemUid, long playerUid)
    {
        equipItemIdToPlayerId[itemUid] = playerUid;
    }

    public void RemoveEquipItemUid(long itemUid)
    {
        equipItemIdToPlayerId.Remove(itemUid);
    }

    public bool TryEquipItem(PlayerData targetPlayerData, Item item)
    {
        if (targetPlayerData == null || item == null)
        {
            return false;
        }

        var result = Tables.Item.TryCanEquip(item.ItemKey, out var canEquip);
        if (!result || !canEquip)
        {
            return false;
        }

        targetPlayerData.Equipment.TryGetEquipUid(item.ItemData.itemSlotType, out var previousItem);

        if (equipItemIdToPlayerId.TryGetValue(item.UniqueId, out var equippedPlayerUid)
            && equippedPlayerUid != targetPlayerData.UniqueId
            && playerDataByKey.TryGetValue(equippedPlayerUid, out var equippedPlayerData))
        {
            equippedPlayerData.Equipment.TryUnequipItem(item);
            equipItemIdToPlayerId.Remove(item.UniqueId);
        }

        if (!targetPlayerData.Equipment.TryEquipItem(item))
        {
            return false;
        }

        if (previousItem != null && previousItem.UniqueId != item.UniqueId)
        {
            equipItemIdToPlayerId.Remove(previousItem.UniqueId);
        }

        equipItemIdToPlayerId[item.UniqueId] = targetPlayerData.UniqueId;
        return true;
    }

    public bool IsEquippedByPlayer(PlayerData targetPlayerData, Item item)
    {
        if (targetPlayerData == null || item == null)
        {
            return false;
        }

        if (!targetPlayerData.Equipment.TryGetEquipUid(item.ItemData.itemSlotType, out var equippedItem))
        {
            return false;
        }

        return equippedItem.UniqueId == item.UniqueId;
    }

    public bool TryUnequipItem(PlayerData targetPlayerData, Item item)
    {
        if (!IsEquippedByPlayer(targetPlayerData, item))
        {
            return false;
        }

        if (!targetPlayerData.Equipment.TryUnequipItem(item))
        {
            return false;
        }

        equipItemIdToPlayerId.Remove(item.UniqueId);
        return true;
    }

    public bool TryGetPlayerDataByPlayerUid(long playerUid, out PlayerData playerData)
    {
        return playerDataByKey.TryGetValue(playerUid, out playerData);
    }

    public bool TryGetPlayerDataByItemUid(long itemUid, out PlayerData playerData)
    {
        playerData = null;
        if (!TryGetPlayerIdByItemId(itemUid, out var playerId))
        {
            return false;
        }

        return TryGetPlayerDataByPlayerUid(playerId, out playerData);
    }
}
