using UnityEngine;

public class PlayerInputLayerProcessor : BaseInputLayerProcessor
{
    BrainActionProcessor brainActionProcessor;

    public override LayerResult ProcessInput(InputContext input)
    {
        if (input.KeyCode == KeyCode.None)
        {
            return RequestBrainAction(new MoveBrainAction(input.Axis));
        }

        if (input.StateType != InputStateType.Pressed)
        {
            return LayerResult.Pass;
        }

        if (input.KeyCode == KeyCode.Z)
        {
            return RequestBrainAction(new PickBrainAction());
        }

        return RequestBrainAction(new UseSkillBrainAction(input.KeyCode));
    }

    LayerResult RequestBrainAction<TAction>(TAction action)
        where TAction : struct, IBrainAction
    {
        RefreshBrainActionProcessor();

        return brainActionProcessor != null && brainActionProcessor.RequestAction(action)
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
