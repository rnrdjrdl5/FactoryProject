using UnityEngine;

public class PlayerFollowAbility : Ability
{
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

    public void Move()
    {
        if (targetPlayer == null || rigidbody2D == null)
        {
            return;
        }

        var delta = targetPlayer.transform.position - transform.position;
        var distance = delta.magnitude;
        if (distance <= followDistance)
        {
            return;
        }

        var dir = delta.normalized;
        var nextPosition = rigidbody2D.position + (Vector2)dir * (followSpeed * Time.fixedDeltaTime);
        rigidbody2D.MovePosition(nextPosition);
    }
}
