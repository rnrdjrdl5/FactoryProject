using UnityEngine;

public class PlayerInputLayerProcessor : BaseInputLayerProcessor
{
    BrainActionProcessor brainActionProcessor;

    public override LayerResult ProcessInput(InputContext input)
    {
        if (input.KeyCode == KeyCode.None)
        {
            return RequestBrainAction(BrainActionRequest.Move(input.Axis));
        }

        if (input.StateType != InputStateType.Pressed)
        {
            return LayerResult.Pass;
        }

        if (input.KeyCode == KeyCode.Z)
        {
            return RequestBrainAction(BrainActionRequest.Pick());
        }

        return RequestBrainAction(BrainActionRequest.UseSkill(input.KeyCode));
    }

    LayerResult RequestBrainAction(BrainActionRequest request)
    {
        RefreshBrainActionProcessor();

        return brainActionProcessor != null && brainActionProcessor.RequestAction(request)
            ? LayerResult.Consume
            : LayerResult.Pass;
    }

    void RefreshBrainActionProcessor()
    {
        if (brainActionProcessor != null)
        {
            return;
        }

        var mainRealm = Entity as MainRealm ?? Entity.GetParent<MainRealm>();
        if (mainRealm != null)
        {
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
}
