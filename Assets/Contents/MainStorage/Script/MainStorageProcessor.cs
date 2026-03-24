public class MainStorageProcessor : Processor
{
    PlayerStorage playerStorage;
    
    public override void Ready()
    {
        base.Ready();

        playerStorage = Entity.GetEntityData<PlayerStorage>();
    }

    public void AddPlayer(Item item)
    {
        playerStorage.AddItem(item);
        if (playerStorage.ItemIdToPlayerId.ContainsKey(item.UniqueId))
        {
            return;
        }
        
        var playerUid = IDLogic.NewUniqueId();
        var playerKey = Tables.Player.GetPlayerByItemKey(item.ItemKey).Key;
        playerStorage.CreateAndAddPlayerData(playerUid, playerKey);
        playerStorage.AddItemToPlayerId(item.UniqueId, playerUid);
    }

    public void RemovePlayerStorage(Item item)
    {
        playerStorage.TryRemoveItem(item.ItemKey, item.Amount);
        if (!playerStorage.ItemIdToPlayerId.ContainsKey(item.UniqueId))
        {
            return;
        }

        if (playerStorage.TryGetPlayerIdByItemId(item.UniqueId, out var playerId))
        {
            playerStorage.RemovePlayerData(playerId);
        }
        playerStorage.RemoveFromItemUid(item.UniqueId);
    }
}
