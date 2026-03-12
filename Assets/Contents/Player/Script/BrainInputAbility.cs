using System;
using UnityEngine;

public class BrainInputAbility : Ability
{
    Brain brain;
    
    PlayerProcessor playerProcessor;
    PlayerPickProcessor playerPickProcessor;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

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
        if (brain.IsAI || playerProcessor == null)
        {
            return;
        }
        
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f || vertical != 0f)
        {
            if (playerProcessor == null)
            {
                return;
            }

            playerProcessor.MoveMessage(horizontal, vertical);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            
        }
    }

    void OnControll(IControlled controlled)
    {
        var controlledEntity = controlled as Entity;
        var processorAbility = controlledEntity.GetAbility<PlayerProcessorAbility>();
        if (processorAbility != null)
        {
            playerProcessor = processorAbility.GetProcessor<PlayerProcessor>();
        }
    }
}
