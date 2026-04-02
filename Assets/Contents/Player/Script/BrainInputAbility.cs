using UnityEngine;

public class BrainInputAbility : Ability, IBrainActionRequestSource
{
    Brain brain;
    IBrainActionRequester actionRequester;
    InputBindingData inputBindingData;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        brain = Entity as Brain;
    }

    public void SetActionRequester(IBrainActionRequester actionRequester)
    {
        this.actionRequester = actionRequester;
    }

    public void SetInputBindingData(InputBindingData inputBindingData)
    {
        this.inputBindingData = inputBindingData;
    }

    private void Update()
    {
        if (brain == null || brain.IsAI || actionRequester == null || inputBindingData == null)
        {
            return;
        }
        
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f || vertical != 0f)
        {
            actionRequester.RequestAction(new PerformCustomActionRequest
            {
                CustomActionType = CustomActionType.Move,
                Direction = new Vector2(horizontal, vertical)
            });
        }

        TryRequestAction(KeyCode.Z);
        TryRequestAction(KeyCode.Mouse0);
        TryRequestAction(KeyCode.Mouse1);
        TryRequestAction(KeyCode.Q);
        TryRequestAction(KeyCode.E);
        TryRequestAction(KeyCode.R);
        TryRequestAction(KeyCode.Space);
        TryRequestAction(KeyCode.LeftShift);
    }

    void TryRequestAction(KeyCode keyCode)
    {
        if (!Input.GetKeyDown(keyCode))
        {
            return;
        }

        if (!inputBindingData.TryGetInputActionType(keyCode, out var inputActionType))
        {
            return;
        }

        RequestInputAction(inputActionType);
    }

    void RequestInputAction(InputActionType inputActionType)
    {
        actionRequester.RequestAction(new PerformInputActionRequest
        {
            InputActionType = inputActionType
        });
    }
}
