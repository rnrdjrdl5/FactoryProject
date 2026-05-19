using UnityEngine;

public enum BrainActionRequestType
{
    Move,
    Pick,
    UseSkill,
    Intent,
}

public struct BrainActionRequest
{
    public BrainActionRequestType RequestType { get; private set; }
    public IntentActionType IntentActionType { get; private set; }
    public Vector2 Direction { get; private set; }
    public KeyCode KeyCode { get; private set; }

    public static BrainActionRequest Move(Vector2 direction = default)
    {
        return new BrainActionRequest
        {
            RequestType = BrainActionRequestType.Move,
            Direction = direction
        };
    }

    public static BrainActionRequest Pick()
    {
        return new BrainActionRequest
        {
            RequestType = BrainActionRequestType.Pick
        };
    }

    public static BrainActionRequest UseSkill(KeyCode keyCode)
    {
        return new BrainActionRequest
        {
            RequestType = BrainActionRequestType.UseSkill,
            KeyCode = keyCode
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
