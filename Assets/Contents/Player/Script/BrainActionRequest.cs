using UnityEngine;

public enum BrainActionRequestType
{
    Input,
    Intent,
}

public struct BrainActionRequest
{
    public BrainActionRequestType RequestType { get; private set; }
    public InputActionType InputActionType { get; private set; }
    public IntentActionType IntentActionType { get; private set; }
    public Vector2 Direction { get; private set; }
    public ContentInputContext<PlayerInputType> ContentInputContext { get; private set; }

    public static BrainActionRequest Input(InputActionType inputActionType, Vector2 direction = default)
    {
        return Input(inputActionType, direction, default);
    }

    public static BrainActionRequest Input(
        InputActionType inputActionType,
        Vector2 direction,
        ContentInputContext<PlayerInputType> contentInputContext)
    {
        return new BrainActionRequest
        {
            RequestType = BrainActionRequestType.Input,
            InputActionType = inputActionType,
            Direction = direction,
            ContentInputContext = contentInputContext
        };
    }

    public static BrainActionRequest Intent(IntentActionType intentActionType)
    {
        return new BrainActionRequest
        {
            RequestType = BrainActionRequestType.Intent,
            IntentActionType = intentActionType
        };
    }
}
