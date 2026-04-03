using UnityEngine;

public class BrainInputAbility : Ability
{
    Brain brain;
    BrainActionProcessor actionProcessor;
    InputBindingData inputBindingData;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        brain = Entity as Brain;
    }

    public void SetActionProcessor(BrainActionProcessor actionProcessor)
    {
        this.actionProcessor = actionProcessor;
    }

    public void SetInputBindingData(InputBindingData inputBindingData)
    {
        this.inputBindingData = inputBindingData;
    }

    private void Update()
    {
        if (brain == null || brain.IsAI || actionProcessor == null)
        {
            return;
        }
        
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f || vertical != 0f)
        {
            actionProcessor.RequestAction(new PerformCustomActionRequest
            {
                CustomActionType = CustomActionType.Move,
                Direction = new Vector2(horizontal, vertical)
            });
        }

    }
}
