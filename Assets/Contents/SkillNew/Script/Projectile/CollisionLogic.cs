using UnityEngine;

public static class CollisionLogic
{
    public static Player FindCollisionTarget(Player caster, Vector3 position, float collisionRadius)
    {
        if (caster == null)
        {
            return null;
        }

        var colliders = Physics2D.OverlapCircleAll(position, collisionRadius, Settings.LayerId.EntityMask);
        foreach (var collider in colliders)
        {
            var targetPlayer = collider.GetComponent<Player>();
            if (targetPlayer == null || targetPlayer == caster)
            {
                continue;
            }

            if (!FactionLogic.IsHostile(caster, targetPlayer))
            {
                continue;
            }

            return targetPlayer;
        }

        return null;
    }
}
