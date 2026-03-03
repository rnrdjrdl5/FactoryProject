using UnityEngine;
using UnityEngine.EventSystems;

public class EntityMoveAbility : Ability
{
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 5;
    MessageBus messageBus;
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        InitializeMessage();
    }

    void InitializeMessage()
    {
        messageBus = Entity.GetEntityData<MessageBus>();
        messageBus.Subscribe<EntityMoveMessage.MoveMessage>(OnMove);
    }

    public override void Uninitialize()
    {
        UninitializeMessage();
        
        base.Uninitialize();
    }

    void UninitializeMessage()
    {
        messageBus.Unsubscribe<EntityMoveMessage.MoveMessage>(OnMove);
    }

    void OnMove(EntityMoveMessage.MoveMessage message)
    {
        Move(new Vector2(message.x, message.y));
    }

    void Move(Vector2 moveDir)
    {
        var nextPosition = rigidbody2D.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigidbody2D.MovePosition(nextPosition);
    }
}
