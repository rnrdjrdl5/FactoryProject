using System;
using UnityEngine;

public class BrainInputAbility : Ability
{
    Brain brain;
    
    PlayerMoveAbility playerMoveAbility;
    PlayerPickProcessor playerPickProcessor;
    PlayerUIProcessor playerUIProcessor;
    
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
        if (brain.IsAI)
        {
            return;
        }
        
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f || vertical != 0f)
        {
            if (playerMoveAbility == null)
            {
                return;
            }

            playerMoveAbility.Move(new Vector2(horizontal, vertical));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (playerPickProcessor != null)
            {
                playerPickProcessor.PickItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            playerUIProcessor.OpenInventory();
        }
    }

    void OnControll(IControlled controlled)
    {
        var controlledEntity = controlled as Entity;
        
        playerMoveAbility = controlledEntity.GetAbility<PlayerMoveAbility>();
        
        var processorAbility = controlledEntity.GetAbility<PlayerProcessorAbility>();
        if (processorAbility != null)
        {
            playerPickProcessor = processorAbility.GetProcessor<PlayerPickProcessor>();
            playerUIProcessor = processorAbility.GetProcessor<PlayerUIProcessor>();
        }
    }
}
