using UnityEngine;

public class CtQSkillContext : BaseSkillContext , IProjectileContext
{
    public Vector2 Direction { get; set; }
    public float ProjectileTime { get; set; }
    public float ProjectileSpeed { get; set; }

    public override void Use()
    {
        base.Use();

        SkillAction.CreateProjectile<CtQProjectileEntity>(this, "Projectile/CtQProjectile");
    }
}