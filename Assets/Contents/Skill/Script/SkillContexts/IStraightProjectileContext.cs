using UnityEngine;

public interface IStraightProjectileContext
{
    Vector2 Direction { get; set; }
    float ProjectileTime { get; set; }
    float ProjectileSpeed { get; set; }
    float Radius { get; set; }

    public Vector3 NextPosition(Vector3 position, float deltaTime)
    {
        return position + (Direction * (ProjectileSpeed * deltaTime)).ToVector3();
    }
}