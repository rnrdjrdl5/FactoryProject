using Newtonsoft.Json;

public class PlayerData : IEntityData, IMessageBus, IUniqueId
{
    [JsonProperty] public Bag Bag { get; private set; }
    [JsonProperty] public Stat Stat { get; private set; }
    [JsonProperty] public Equipment Equipment { get; private set; }
    [JsonProperty] public Faction Faction { get; private set; }
    [JsonIgnore] public MessageBus MessageBus { get; set; }
    [JsonProperty] public long UniqueId { get; set; }
    
    // [JsonProperty] public string PlayerKey { get; set; }
    // [JsonIgnore] public Tables.Player TableData => Tables.Player.Get(PlayerKey);

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
        
        //Stat.AddStats(TableData);
    }

    public void Uninitialize()
    {
        if (MessageBus != null)
        {
            MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
            MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
        }
        
        //Stat.RemoveStats(TableData);

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

            MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
            MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
            MessageBus.Subscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
            MessageBus.Subscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
        }
    }
    
    void OnEquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
    }
    
    void OnUnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        
    }

    public static PlayerData Create(MessageBus messageBus, string playerKey, long uniqueId = 0)
    {
        var playerData = new PlayerData();
        playerData.UniqueId = uniqueId == 0 ? IDLogic.NewUniqueId() : uniqueId;
        //playerData.PlayerKey = playerKey;
        playerData.Initialize();
        playerData.MessageBus = messageBus;
        playerData.OnSetMessageBus();
        
        return playerData;
    }
}
