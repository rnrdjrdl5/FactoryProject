using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class PlayerItem : Item
{
    public PlayerItem(string itemKey, int amount, long playerUid) : base(itemKey, amount)
    {
        playerUniqueId = playerUid;
    }

    public long PlayerUniqueId => playerUniqueId;
    
    [JsonProperty] long playerUniqueId;

    public static PlayerItem Create(string itemKey, int amount, long playerUid)
    {
        var playerItem = new PlayerItem(itemKey, amount, playerUid);
        return playerItem;
    }
}