public class CtQSkillProcessor : SkillProcessor
{
    public override void OnReadySkillContext()
    {
        base.OnReadySkillContext();

        SkillAction.CreateSkillEntity<CtQProjectileEntity>(SkillContext, CtQProjectileEntity.PrefabPath, Realm);
        
        ProcessorAbility.RemoveProcessor(this);
    }
}