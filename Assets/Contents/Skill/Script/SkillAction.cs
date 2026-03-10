public static class SkillAction
{
    public static EntityType CreateSkillEntity<EntityType>(BaseSkillContext skillContext, string prefabPath, Entity parent)
        where EntityType : Entity, new()
    {
        var projectileEntity = parent.AddEntity<EntityType>(prefabPath);
        var processorAbility = projectileEntity.GetAbility<ProcessorAbility>();
        var processors = processorAbility.GetUpdateProcessors<SkillProcessor>();

        foreach (var processor in processors)
        {
            processor.SetSkillContext(skillContext);
        }
        
        return projectileEntity;
    }
}