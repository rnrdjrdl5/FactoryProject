using Newtonsoft.Json;
using UnityEngine;

public class PhysicalInputStateData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    public Vector2 MoveDirection { get; private set; }
    public Vector3 MouseScreenPosition { get; private set; }

    public void Initialize(IInitData initData = null)
    {
        MoveDirection = Vector2.zero;
        MouseScreenPosition = Vector3.zero;
    }

    public void Uninitialize()
    {
        MoveDirection = Vector2.zero;
        MouseScreenPosition = Vector3.zero;
    }

    public void OnSetMessageBus()
    {
    }

    public bool SetMoveDirection(Vector2 moveDirection)
    {
        if (MoveDirection == moveDirection)
        {
            return false;
        }

        MoveDirection = moveDirection;
        return true;
    }

    public bool SetMouseScreenPosition(Vector3 mouseScreenPosition)
    {
        if (MouseScreenPosition == mouseScreenPosition)
        {
            return false;
        }

        MouseScreenPosition = mouseScreenPosition;
        return true;
    }
}
