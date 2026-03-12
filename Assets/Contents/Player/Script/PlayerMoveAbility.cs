using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveAbility : Ability
{
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 5;

    public void Move(Vector2 moveDir)
    {
        var nextPosition = rigidbody2D.position + moveDir * (moveSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(nextPosition);
    }
}