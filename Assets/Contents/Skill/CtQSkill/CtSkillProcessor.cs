[SkillProcessorKey("CtQ")]
public class CtQSkillProcessor : SkillProcessor
{
    public override void OnReadySkillContext()
    {
        base.OnReadySkillContext();

        SkillLogic.CreateSkillEntity<CtQProjectileEntity>(SkillContext, CtQProjectileEntity.PrefabPath, Realm);
        
        ProcessorAbility.RemoveProcessor(this);
    }
}
