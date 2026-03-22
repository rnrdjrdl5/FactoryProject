public class MainStorageProcessor : Processor
{
    PlayerStorage playerStorage;
    
    public override void Ready()
    {
        base.Ready();

        playerStorage = Entity.GetEntityData<PlayerStorage>();
    }

    public void AddPlayerStorage(Item item)
    {
        playerStorage.AddItem(item);
        if (playerStorage.ItemIdToPlayerId.ContainsKey(item.UniqueId))
        {
            return;
        }
        
        var playerKey = IDLogic.NewUniqueId();
        playerStorage.CreateAndAddPlayerData(playerKey);
        playerStorage.AddItemToPlayerId(item.UniqueId, playerKey);
    }

    public void RemovePlayerStorage(Item item)
    {
        playerStorage.TryRemoveItem(item.ItemKey, item.Amount);
        if (!playerStorage.ItemIdToPlayerId.ContainsKey(item.UniqueId))
        {
            return;
        }

        if (playerStorage.TryGetPlayerId(item.UniqueId, out var playerId))
        {
            playerStorage.RemovePlayerData(playerId);
        }
        playerStorage.RemoveFromItemUid(item.UniqueId);
    }
}
