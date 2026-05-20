using UnityEngine;

public class StraightProjectile : ProjectileEntity, IDirectionProvider
{
    public Vector3 Direction => moveDirection;

    Vector3 moveDirection;
    float speed;

    protected override void InitializeProjectile()
    {
        base.InitializeProjectile();

        var projectileParam = SkillContext.SkillData.ParsedActionParam as SkillActionProjectileParam;
        speed = projectileParam.Speed.Value;
        
        if (SkillContext?.TargetPosition == null)
        {
            moveDirection = Vector3.zero;
            return;
        }

        moveDirection = (SkillContext.TargetPosition.Value - transform.position).normalized;
    }

    protected override void MoveProjectile()
    {
        transform.position += moveDirection * (speed * Time.deltaTime);
        CheckCollision();
    }

    void CheckCollision()
    {
        var originCaster = SkillContext.OriginCaster as Player;
        if (originCaster == null)
        {
            return;
        }

        var targetPlayer = CollisionLogic.FindCollisionTarget(originCaster, transform.position, CollisionRadius);
        if (targetPlayer == null)
        {
            return;
        }

        ProcessCollision(targetPlayer);
        if (Parent == null)
        {
            return;
        }
    }
}
