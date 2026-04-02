using UnityEngine;

public interface IBrainActionRequest
{
}

public interface IBrainActionRequester
{
    void RequestAction(IBrainActionRequest request);
}

public interface IBrainActionRequestSource
{
    void SetActionRequester(IBrainActionRequester actionRequester);
}

public struct PerformInputActionRequest : IBrainActionRequest
{
    public InputActionType InputActionType { get; set; }
}

public struct PerformIntentActionRequest : IBrainActionRequest
{
    public IntentActionType IntentActionType { get; set; }
}

public struct PerformCustomActionRequest : IBrainActionRequest
{
    public CustomActionType CustomActionType { get; set; }
    public Vector2 Direction { get; set; }
}

public struct PerformSystemActionRequest : IBrainActionRequest
{
    public SystemActionType SystemActionType { get; set; }
}

public struct UseSkillRequest : IBrainActionRequest
{
    public string SkillKey { get; set; }
}
