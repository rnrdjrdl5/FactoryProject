using UnityEngine;

public class BrainInputAbility : Ability
{
    Brain brain;
    BrainActionProcessor brainActionProcessor;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        brain = Entity as Brain;
        var processorAbility = Entity.GetAbility<BrainProcessorAbility>();
        brainActionProcessor = processorAbility?.GetProcessor<BrainActionProcessor>();
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
            brainActionProcessor?.Move(new Vector2(horizontal, vertical));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            brainActionProcessor?.TryPick();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            brainActionProcessor?.TryUseUniqueSkill();
        }
    }
}
