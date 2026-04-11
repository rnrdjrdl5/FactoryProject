using UnityEngine;

public class PlayerFollowAbility : Ability
{
    public Player TargetPlayer => targetPlayer;
    public float FollowDistance => followDistance;

    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] float followDistance = 2f;
    [SerializeField] float followSpeed = 5f;

    Player targetPlayer;

    public void SetTarget(Player target)
    {
        targetPlayer = target;
    }

    public void ClearTarget()
    {
        targetPlayer = null;
    }

    public void SetFollowDistance(float distance)
    {
        followDistance = distance;
    }

    public void SetFollowSpeed(float speed)
    {
        followSpeed = speed;
    }

    public override void Uninitialize()
    {
        targetPlayer = null;
        
        base.Uninitialize();
    }

    public Vector2 Move()
    {
        if (targetPlayer == null || rigidbody2D == null)
        {
            return Vector2.zero;
        }

        var delta = targetPlayer.transform.position - transform.position;
        var distance = delta.magnitude;
        if (distance <= followDistance)
        {
            return Vector2.zero;
        }

        var dir = delta.normalized;
        var moveDelta = (Vector2)dir * (followSpeed * Time.fixedDeltaTime);
        var nextPosition = rigidbody2D.position + moveDelta;
        rigidbody2D.MovePosition(nextPosition);

        return moveDelta;
    }
}
