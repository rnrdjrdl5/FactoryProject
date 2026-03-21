using Newtonsoft.Json;

public class PlayerData : IEntityData, IMessageBus, IUniqueId
{
    [JsonProperty] public Bag Bag { get; private set; }
    [JsonProperty] public Stat Stat { get; private set; }
    [JsonProperty] public Equipment Equipment { get; private set; }
    [JsonProperty] public Faction Faction { get; private set; }
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    [JsonProperty] public long UniqueId { get; set; }

    public void Initialize(IInitData initData = null)
    {
        Bag = new Bag();
        Bag.Initialize(initData);

        Stat = new Stat();
        Stat.Initialize(initData);

        Equipment = new Equipment();
        Equipment.Initialize(initData);

        Faction = new Faction();
        Faction.Initialize(initData);
    }

    public void Uninitialize()
    {
        Bag?.Uninitialize();
        Stat?.Uninitialize();
        Equipment?.Uninitialize();
        Faction?.Uninitialize();
    }

    public void OnSetMessageBus()
    {
        if (MessageBus == null)
        {
            return;
        }

        if (Bag != null)
        {
            Bag.MessageBus = MessageBus;
            Bag.OnSetMessageBus();
        }

        if (Equipment != null)
        {
            Equipment.MessageBus = MessageBus;
            Equipment.OnSetMessageBus();
        }
    }

    public static PlayerData Create(long uniqueId = 0)
    {
        var playerData = new PlayerData();
        playerData.UniqueId = uniqueId == 0 ? IDLogic.NewUniqueId() : uniqueId;
        playerData.Initialize();
        
        return playerData;
    }
}
