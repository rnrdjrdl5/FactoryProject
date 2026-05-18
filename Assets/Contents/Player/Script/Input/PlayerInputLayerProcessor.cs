using UnityEngine;

public class PlayerInputLayerProcessor : BaseContentInputLayerProcessor<PlayerInputType>
{
    BrainActionProcessor brainActionProcessor;

    protected override bool TryMapContentInput(TokenInputContext tokenInput, out ContentInputContext<PlayerInputType> contentInput)
    {
        switch (tokenInput.InputType)
        {
            case TokenInputType.MoveAxis:
                contentInput = new ContentInputContext<PlayerInputType>(PlayerInputType.Move, tokenInput);
                return true;
            case TokenInputType.Action1:
                return TryMapStartedInput(tokenInput, PlayerInputType.Pick, out contentInput);
            case TokenInputType.PointerPrimary:
                return TryMapStartedInput(tokenInput, PlayerInputType.MainAttack, out contentInput);
            case TokenInputType.PointerSecondary:
                return TryMapStartedInput(tokenInput, PlayerInputType.SubAttack, out contentInput);
            case TokenInputType.Action2:
                return TryMapStartedInput(tokenInput, PlayerInputType.Skill1, out contentInput);
            case TokenInputType.Action3:
                return TryMapStartedInput(tokenInput, PlayerInputType.Skill2, out contentInput);
            case TokenInputType.Action4:
                return TryMapStartedInput(tokenInput, PlayerInputType.Skill3, out contentInput);
            case TokenInputType.Action5:
                return TryMapStartedInput(tokenInput, PlayerInputType.MainUtility, out contentInput);
            case TokenInputType.Action6:
                return TryMapStartedInput(tokenInput, PlayerInputType.SubUtility, out contentInput);
            default:
                contentInput = default;
                return false;
        }
    }

    protected override LayerResult ProcessContentInput(ContentInputContext<PlayerInputType> contentInput)
    {
        if (!TryGetInputActionType(contentInput.InputType, out var inputActionType))
        {
            return LayerResult.Pass;
        }

        RefreshBrainActionProcessor();
        if (brainActionProcessor == null)
        {
            return LayerResult.Pass;
        }

        return brainActionProcessor.RequestAction(BrainActionRequest.Input(inputActionType, GetInputDirection(contentInput), contentInput))
            ? LayerResult.Consume
            : LayerResult.Pass;
    }

    bool TryMapStartedInput(TokenInputContext tokenInput, PlayerInputType inputType, out ContentInputContext<PlayerInputType> contentInput)
    {
        if (tokenInput.RawContext.InputType != RawInputType.Started)
        {
            contentInput = default;
            return false;
        }

        contentInput = new ContentInputContext<PlayerInputType>(inputType, tokenInput);
        return true;
    }

    bool TryGetInputActionType(PlayerInputType playerInputType, out InputActionType inputActionType)
    {
        switch (playerInputType)
        {
            case PlayerInputType.Move:
                inputActionType = InputActionType.Move;
                return true;
            case PlayerInputType.Pick:
                inputActionType = InputActionType.Pick;
                return true;
            case PlayerInputType.MainAttack:
                inputActionType = InputActionType.MainAttack;
                return true;
            case PlayerInputType.SubAttack:
                inputActionType = InputActionType.SubAttack;
                return true;
            case PlayerInputType.Skill1:
                inputActionType = InputActionType.Skill1;
                return true;
            case PlayerInputType.Skill2:
                inputActionType = InputActionType.Skill2;
                return true;
            case PlayerInputType.Skill3:
                inputActionType = InputActionType.Skill3;
                return true;
            case PlayerInputType.MainUtility:
                inputActionType = InputActionType.MainUtility;
                return true;
            case PlayerInputType.SubUtility:
                inputActionType = InputActionType.SubUtility;
                return true;
            default:
                inputActionType = default;
                return false;
        }
    }

    Vector2 GetInputDirection(ContentInputContext<PlayerInputType> contentInput)
    {
        return contentInput.InputType == PlayerInputType.Move ? contentInput.TokenContext.RawContext.Axis : Vector2.zero;
    }

    void RefreshBrainActionProcessor()
    {
        brainActionProcessor = null;

        var mainRealm = Entity as MainRealm ?? Entity.GetParent<MainRealm>();
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
