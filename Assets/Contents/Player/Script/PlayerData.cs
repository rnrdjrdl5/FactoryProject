public class PlayerData : IEntityData, IMessageBus
{
    public Bag Bag { get; private set; }
    public Stat Stat { get; private set; }
    public Equipment Equipment { get; private set; }
    public Faction Faction { get; private set; }

    public MessageBus MessageBus { get; set; }

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
}
