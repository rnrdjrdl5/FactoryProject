using System.Collections.Generic;
using Newtonsoft.Json;

// 플레이어 데이터 + 아이템 통합 Storage
public class PlayerStorage : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<long, PlayerData> playerDataByKey = new();
    [JsonProperty] Dictionary<long, long> itemIdToPlayerId = new();
    [JsonProperty] Inventory inventory;

    public IReadOnlyDictionary<long, PlayerData> PlayerDataByKey => playerDataByKey;
    public IReadOnlyDictionary<long, long> ItemIdToPlayerId => itemIdToPlayerId;
    public IReadOnlyList<Item> Items => inventory?.Items;

    public void Initialize(IInitData initData = null)
    {
        playerDataByKey.Clear();
        itemIdToPlayerId.Clear();

        if (inventory == null)
        {
            inventory = Inventory.Create(Tables.ItemType.Player);
        }
        else
        {
            inventory.Initialize(Tables.ItemType.Player);
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

    public void Equip(string itemKey)
    {
        inventory.Equip(itemKey);
    }

    public void Equip(Item item)
    {
        inventory.Equip(item);
    }

    public void Unequip(string itemKey)
    {
        inventory.Unequip(itemKey);
    }

    public void Unequip(Item item)
    {
        inventory.Unequip(item);
    }

    public void CreateAndAddPlayerData(long playerKey)
    {
        playerDataByKey.TryAdd(playerKey, PlayerData.Create(playerKey));
    }

    public bool TryGetPlayerData(long playerKey, out PlayerData playerData)
    {
        return playerDataByKey.TryGetValue(playerKey, out playerData);
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

    public bool TryGetPlayerId(long itemUid, out long playerId)
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
}
