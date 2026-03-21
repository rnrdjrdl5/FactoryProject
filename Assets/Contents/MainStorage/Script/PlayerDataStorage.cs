using System.Collections.Generic;
using Newtonsoft.Json;

// 플레이어 데이터 Storage
public class PlayerDataStorage : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    [JsonProperty] Dictionary<long, PlayerData> playerDataByKey = new();

    public IReadOnlyDictionary<long, PlayerData> PlayerDataByKey => playerDataByKey;

    public void Initialize(IInitData initData = null)
    {
        playerDataByKey.Clear();
    }

    public void Uninitialize()
    {
        foreach (var playerData in playerDataByKey.Values)
        {
            playerData.Uninitialize();
        }

        playerDataByKey.Clear();
    }

    public void OnSetMessageBus()
    {
        if (MessageBus == null)
        {
            return;
        }

        foreach (var playerData in playerDataByKey.Values)
        {
            playerData.MessageBus = MessageBus;
            playerData.OnSetMessageBus();
        }
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
}
