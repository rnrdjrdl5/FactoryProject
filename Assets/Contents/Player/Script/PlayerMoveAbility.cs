using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveAbility : Ability
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
        messageBus.Subscribe<PlayerMoveMessage.MoveMessage>(OnMove);
    }

    public override void Uninitialize()
    {
        UninitializeMessage();
        
        base.Uninitialize();
    }

    void UninitializeMessage()
    {
        messageBus.Unsubscribe<PlayerMoveMessage.MoveMessage>(OnMove);
    }

    void OnMove(PlayerMoveMessage.MoveMessage message)
    {
        Move(new Vector2(message.x, message.y));
    }

    void Move(Vector2 moveDir)
    {
        var nextPosition = rigidbody2D.position + moveDir * (moveSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(nextPosition);
    }
}
