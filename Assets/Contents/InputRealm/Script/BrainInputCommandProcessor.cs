using UnityEngine;

public class BrainInputCommandProcessor : Processor
{
    BrainActionProcessor brainActionProcessor;
    PhysicalInputStateData inputStateData;

    public override void Ready()
    {
        base.Ready();

        inputStateData = Entity.GetEntityData<PhysicalInputStateData>();
        RefreshActionProcessor();
    }

    public override void Uninitialize()
    {
        brainActionProcessor = null;
        inputStateData = null;

        base.Uninitialize();
    }

    public void RequestAction(InputActionType inputActionType)
    {
        RefreshActionProcessor();
        brainActionProcessor?.RequestAction(BrainActionRequest.Input(inputActionType, GetInputDirection(inputActionType)));
    }

    Vector2 GetInputDirection(InputActionType inputActionType)
    {
        switch (inputActionType)
        {
            case InputActionType.Move:
                return inputStateData?.AxisDirection ?? Vector2.zero;
            default:
                return Vector2.zero;
        }
    }

    void RefreshActionProcessor()
    {
        // TODO : 수정 필요
        
        var mainRealm = Entity.GetParent<MainRealm>();
        if (mainRealm == null)
        {
            return;
        }

        foreach (var brain in mainRealm.GetChildren<Brain>())
        {
            if (brain == null || brain.ControlMode != BrainControlMode.PlayerInput)
            {
                continue;
            }

            var processorAbility = brain.GetAbility<BrainProcessorAbility>();
            if (processorAbility?.GetProcessor<BrainActionProcessor>() is BrainActionProcessor playerBrainActionProcessor)
            {
                brainActionProcessor = playerBrainActionProcessor;
                return;
            }
        }
    }
}
