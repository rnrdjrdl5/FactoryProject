using System.Collections.Generic;

// 플레이어(아이템) 보관함
public class PlayerItemStorage : Inventory, IEntityData, IMessageBus
{
    public MessageBus MessageBus { get; set; }
    public IReadOnlyDictionary<long, long> ItemIdToPlayerId => itemIdToPlayerId;  

    Dictionary<long, long> itemIdToPlayerId = new(); 
    
    public void Initialize(IInitData initData = null)
    {
    }

    public void Uninitialize()
    {
        
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