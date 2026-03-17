public class AutoAttackFlow : ProcessorFlow
{
    public float Duration { get; private set; } = 1;
        
    SkillAbility skillAbility;

    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        var brain = Processor.Entity as Brain;
        var entity = brain.Controll as Entity;
        skillAbility = entity.GetAbility<SkillAbility>();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        if (elapsedTime >= Duration)
        {
            UseSkill();
            Finish();
        }
    }

    void UseSkill()
    {
        if (skillAbility == null)
        {
            return;
        }
        
        skillAbility.UseSkill();
    }
}