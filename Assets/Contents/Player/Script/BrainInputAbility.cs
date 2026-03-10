using System;
using UnityEngine;

public class BrainInputAbility : Ability
{
    Brain brain;
    PlayerProcessor processor;
    
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
            if (processor == null)
            {
                return;
            }

            processor.MoveMessage(horizontal, vertical);
        }
    }

    void OnControll(IControlled controlled)
    {
        var controlledEntity = controlled as Entity;
        var processorAbility = controlledEntity.GetAbility<PlayerProcessorAbility>();
        if (processorAbility != null)
        {
            processor = processorAbility.GetProcessor<PlayerProcessor>();
        }
    }
}

