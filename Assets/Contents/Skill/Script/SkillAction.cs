public static class SkillAction
{
    public static ProjectileEntityType CreateProjectile<ProjectileEntityType>(BaseSkillContext skillContext, string prefabPath, Entity parent = null)
        where ProjectileEntityType : ProjectileEntity, new()
    {
        var projectileEntity = ProjectileEntity.Create<ProjectileEntityType>(skillContext, prefabPath, parent);
        
        return projectileEntity;
    }
}