using UnityEngine;

public class PlayerMoveAbility : Ability
{
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 5;

    public Vector2 Move(Vector2 moveDir)
    {
        if (moveDir == Vector2.zero)
        {
            return Vector2.zero;
        }

        var moveDelta = moveDir.normalized * (moveSpeed * Time.fixedDeltaTime);
        var nextPosition = rigidbody2D.position + moveDelta;
        rigidbody2D.MovePosition(nextPosition);

        return moveDelta;
    }
}
