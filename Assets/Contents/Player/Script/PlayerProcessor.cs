public class PlayerProcessor : Processor
{
    MessageBus messageBus;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
        messageBus = Entity.GetEntityData<MessageBus>();
    }

    public void MoveMessage(float x, float y)
    {
        if (messageBus == null)
        {
            return;
        }
        
        var message = new PlayerMoveMessage.MoveMessage();
        message.x = x;
        message.y = y;
        
        messageBus.Publish(message);
    }
}
