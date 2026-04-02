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

public struct MoveActionRequest : IBrainActionRequest
{
    public Vector2 Direction { get; set; }
}

public struct PickActionRequest : IBrainActionRequest
{
}

public struct UseSkillSlotActionRequest : IBrainActionRequest
{
    public SkillSlotType SkillSlotType { get; set; }
}

public struct FollowTargetActionRequest : IBrainActionRequest
{
}
