[SkillProcessorKey("CtQ")]
public class CtQSkillProcessor : SkillProcessor
{
    public override void OnReadySkillContext()
    {
        base.OnReadySkillContext();

        var entity = SkillLogic.CreateSkillEntity<CtQProjectileEntity>(SkillContext, CtQProjectileEntity.PrefabPath, Realm);
        entity.transform.position = Entity.transform.position;
            
        ProcessorAbility.RemoveProcessor(this);
    }
}
