using UnityEngine;

public class CtQSkillContext : BaseSkillContext , IStraightProjectileContext
{
    public Vector2 Direction { get; set; }
    public float ProjectileTime { get; set; }
    public float ProjectileSpeed { get; set; }
    public float Radius { get; set; }
}