using Newtonsoft.Json;

public class PlayerData : IEntityData, IMessageBus, IUniqueId
{
    [JsonProperty] public Bag Bag { get; private set; }
    [JsonProperty] public Buff Buff { get; private set; }
    [JsonProperty] public Stat Stat { get; private set; }
    [JsonProperty] public Equipment Equipment { get; private set; }
    [JsonProperty] public Faction Faction { get; private set; }
    [JsonProperty] public InputBindingData InputBindingData { get; private set; }
    [JsonProperty] public InputActionSkillData InputActionSkillData { get; private set; }
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

        Buff = new Buff();
        Buff.Initialize(initData);

        Stat = new Stat();
        Stat.Initialize(initData);
        RefreshBaseStats();

        Equipment = new Equipment();
        Equipment.Initialize(initData);

        Faction = new Faction();
        Faction.Initialize(initData);

        InputBindingData = new InputBindingData();
        InputBindingData.Initialize(initData);

        InputActionSkillData = new InputActionSkillData();
        InputActionSkillData.Initialize(initData);

        RefreshDefaultInputActionSkills();
    }

    public void Uninitialize()
    {
        if (MessageBus != null)
        {
            MessageBus.Unsubscribe<EntityDataMsg.EquipmentEquipMsg>(OnEquipmentEquip);
            MessageBus.Unsubscribe<EntityDataMsg.UnequipmentEquipMsg>(OnUnequipmentEquip);
        }

        Bag?.Uninitialize();
        Buff?.Uninitialize();
        Stat?.Uninitialize();
        Equipment?.Uninitialize();
        Faction?.Uninitialize();
        InputBindingData?.Uninitialize();
        InputActionSkillData?.Uninitialize();
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

        if (Buff != null)
        {
            Buff.MessageBus = MessageBus;
            Buff.OnSetMessageBus();
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

        if (InputBindingData != null)
        {
            InputBindingData.MessageBus = MessageBus;
            InputBindingData.OnSetMessageBus();
        }

        if (InputActionSkillData != null)
        {
            InputActionSkillData.MessageBus = MessageBus;
            InputActionSkillData.OnSetMessageBus();
        }
    }
    
    void OnEquipmentEquip(EntityDataMsg.EquipmentEquipMsg msg)
    {
        if (msg.Equipment != Equipment)
        {
            return;
        }

        if (Stat == null || msg.Item?.ItemData == null)
        {
            return;
        }

        Stat.AddStats(GetEquipmentStatSourceKey(msg.Item), msg.Item.ItemData);
        RefreshEquippedWeaponInputAction(msg.Item);
    }
    
    void OnUnequipmentEquip(EntityDataMsg.UnequipmentEquipMsg msg)
    {
        if (msg.Equipment != Equipment)
        {
            return;
        }

        if (Stat == null || msg.Item == null)
        {
            return;
        }

        Stat.RemoveStats(GetEquipmentStatSourceKey(msg.Item));
        ClearEquippedWeaponInputAction(msg.Item);
    }

    void RefreshBaseStats()
    {
        if (Stat == null || string.IsNullOrEmpty(PlayerKey))
        {
            return;
        }

        var tableData = TableData;
        if (tableData == null)
        {
            return;
        }

        Stat.AddStats(new StatSourceKey(StatSourceType.Player, PlayerKey), tableData);
    }

    void RefreshDefaultInputActionSkills()
    {
        if (InputActionSkillData == null)
        {
            return;
        }

        var tableData = TableData;
        if (tableData == null || string.IsNullOrWhiteSpace(tableData.uniqueSkillKey))
        {
            return;
        }

        InputActionSkillLogic.TrySetSkillKey(this, InputActionType.MainAttack, tableData.uniqueSkillKey);
    }

    void RefreshEquippedWeaponInputAction(Item item)
    {
        if (item?.ItemData == null || item.ItemData.itemType != Tables.ItemType.Weapon)
        {
            return;
        }

        InputActionSkillLogic.TrySetSkillKey(this, InputActionType.SubAttack, item.ItemData.uniqueSkillKey);
    }

    void ClearEquippedWeaponInputAction(Item item)
    {
        if (item?.ItemData == null || item.ItemData.itemType != Tables.ItemType.Weapon)
        {
            return;
        }

        InputActionSkillLogic.TryClearSkillKey(this, InputActionType.SubAttack);
    }

    StatSourceKey GetEquipmentStatSourceKey(Item item)
    {
        return new StatSourceKey(StatSourceType.Equipment, item.UniqueId.ToString());
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
