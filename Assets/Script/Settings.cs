using UnityEngine;

public static class Settings
{
    public static class LayerId
    {
        public static string Entity = nameof(Entity);
        public static string Projectile = nameof(Projectile);
        
        public static int EntityMask => LayerMask.GetMask(Entity);
        public static int ProjectileMask => LayerMask.GetMask(Projectile);
    }
}