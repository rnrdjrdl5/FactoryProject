using UnityEngine;

public interface IProjectileContext
{
    Vector2 Direction { get; set; }
    float ProjectileTime { get; set; }
    float ProjectileSpeed { get; set; }

    public Vector3 NextPosition(Vector3 position, float deltaTime)
    {
        return position + (Direction * (ProjectileSpeed * deltaTime)).ToVector3();
    }
}