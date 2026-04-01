using Newtonsoft.Json;

public class PlayerData : IEntityData, IMessageBus, IUniqueId
{
    [JsonProperty] public Bag Bag { get; private set; }
    [JsonProperty] public Stat Stat { get; private set; }
    [JsonProperty] public Equipment Equipment { get; private set; }
    [JsonProperty] public Faction Faction { get; private set; }
    [JsonProperty] public string PlayerKey { get; private set; }
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    [JsonProperty] public long UniqueId { get; set; }
    [JsonIgnore] public Tables.Player TableData => string.IsNullOrEmpty(PlayerKey) ? null : Tables.Player.Get(PlayerKey);

    public void Initialize(IInitData initData = null)
    {
        if (initData is IUniqueId uniqueIdData)
        {
            UniqueId = uniqueIdData.UniqueId;
        }

        if (initData is PlayerInitData playerInitData)
        {
            PlayerKey = playerInitData.PlayerKey;
        }

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
        if (MessageBus != null)
        {
            MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
            MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
        }

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

        if (Stat != null)
        {
            Stat.MessageBus = MessageBus;
            Stat.OnSetMessageBus();
        }

        if (Equipment != null)
        {
            Equipment.MessageBus = MessageBus;
            Equipment.OnSetMessageBus();

            MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
            MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
        }
    }
    
    void OnEquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        if (msg.Equipment != Equipment)
        {
            return;
        }
    }
    
    void OnUnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        if (msg.Equipment != Equipment)
        {
            return;
        }
    }

    public static PlayerData Create(MessageBus messageBus, string playerKey, long uniqueId = 0)
    {
        var playerData = new PlayerData();
        var resolvedUniqueId = uniqueId == 0 ? IDLogic.NewUniqueId() : uniqueId;
        playerData.Initialize(new PlayerInitData
        {
            UniqueId = resolvedUniqueId,
            PlayerKey = playerKey
        });
        playerData.MessageBus = messageBus;
        playerData.OnSetMessageBus();
        
        return playerData;
    }
}
