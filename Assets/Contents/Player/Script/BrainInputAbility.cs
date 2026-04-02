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
            actionRequester.RequestAction(new MoveActionRequest
            {
                Direction = new Vector2(horizontal, vertical)
            });
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            actionRequester.RequestAction(new PickActionRequest());
        }

        TryRequestSkillSlot(KeyCode.Mouse0);
        TryRequestSkillSlot(KeyCode.Mouse1);
        TryRequestSkillSlot(KeyCode.Q);
        TryRequestSkillSlot(KeyCode.E);
        TryRequestSkillSlot(KeyCode.R);
        TryRequestSkillSlot(KeyCode.Space);
        TryRequestSkillSlot(KeyCode.LeftShift);
    }

    void TryRequestSkillSlot(KeyCode keyCode)
    {
        if (!Input.GetKeyDown(keyCode))
        {
            return;
        }

        if (!inputBindingData.TryGetSkillSlotType(keyCode, out var skillSlotType))
        {
            return;
        }

        actionRequester.RequestAction(new UseSkillSlotActionRequest
        {
            SkillSlotType = skillSlotType
        });
    }
}
