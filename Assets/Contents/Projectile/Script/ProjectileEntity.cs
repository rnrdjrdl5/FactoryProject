using UnityEngine;

public class ProjectileEntity : Entity
{
    public BaseSkillContext SkillContext => skillContext;
    
    // NOTE : 일반 Projectile과 구분 필요 시 분리 필요
    BaseSkillContext skillContext;
    
    public void SetSkillContext(BaseSkillContext skillContext)
    {
        this.skillContext = skillContext;
    }

    public static ProjectileEntityType Create<ProjectileEntityType>(BaseSkillContext skillContext, string prefabPath, Entity parent = null) where ProjectileEntityType : ProjectileEntity , new()
    {
        if (parent == null)
        {
            parent = skillContext.Realm;
        }
        
        var projectileEntity = parent.AddEntity<ProjectileEntityType>(prefabPath, OnCreate: entity =>
        {
            entity.SetSkillContext(skillContext);
        });

        return projectileEntity;
    }
}