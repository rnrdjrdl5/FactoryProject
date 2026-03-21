public class TeamInventory : IEntityData, IMessageBus
{
    public MessageBus MessageBus { get; set; }
    public Inventory Inventory => inventory;
    
    Inventory inventory = new();
    
    public void Initialize(IInitData initData = null)
    {
    }

    public void Uninitialize()
    {
        
    }
    
    public void OnSetMessageBus()
    {
        inventory.MessageBus = MessageBus;
        inventory.OnSetMessageBus();
    }
}