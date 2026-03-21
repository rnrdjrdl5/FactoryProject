public class MainStorageProcessor : Processor
{
    PlayerItemStorage playerItemStorage;
    PlayerDataStorage playerDataStorage;
    
    public override void Ready()
    {
        base.Ready();

        playerItemStorage = Entity.GetEntityData<PlayerItemStorage>();
        playerDataStorage = Entity.GetEntityData<PlayerDataStorage>();
    }

    public void AddPlayerStorage(Item item)
    {
        playerItemStorage.AddItem(item);
        if (playerItemStorage.ItemIdToPlayerId.ContainsKey(item.UniqueId))
        {
            return;
        }
        
        var playerKey = IDLogic.NewUniqueId();
        playerDataStorage.CreateAndAddPlayerData(playerKey);
        playerItemStorage.AddItemToPlayerId(item.UniqueId, playerKey);
    }

    public void RemovePlayerStorage(Item item)
    {
        playerItemStorage.TryRemoveItem(item.ItemKey, item.Amount);
        if (!playerItemStorage.ItemIdToPlayerId.ContainsKey(item.UniqueId))
        {
            return;
        }

        if (playerItemStorage.TryGetPlayerId(item.UniqueId, out var playerId))
        {
            playerDataStorage.RemovePlayerData(playerId);
        }
        playerItemStorage.RemoveFromItemUid(item.UniqueId);
    }
}