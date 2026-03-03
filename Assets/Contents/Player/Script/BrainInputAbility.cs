using System;
using UnityEngine;

public class BrainInputAbility : Ability
{
    Brain brain;
    Entity controlledEntity;
    MessageBus messageBus;
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        brain = Entity as Brain;

        brain.OnAttachControll += OnControll;
    }

    public override void Uninitialize()
    {
        brain.OnAttachControll -= OnControll;
        
        base.Uninitialize();
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f || vertical != 0f)
        {
            if (messageBus == null)
            {
                return;
            }
            
            var message = new EntityMoveMessage.MoveMessage();
            message.x = horizontal;
            message.y = vertical;
            
            messageBus.Publish(message);
        }
    }

    void OnControll(IControlled controlled)
    {
        controlledEntity = controlled as Entity;
        messageBus = controlledEntity.GetEntityData<MessageBus>();
    }
}

