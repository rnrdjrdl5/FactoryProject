public class FollowTargetFlow : ProcessorFlow
{
    PlayerFollowAbility followAbility;
    
    public override void OnEnterFlow()
    {
        base.OnEnterFlow();

        var brain = Processor.Entity as Brain;
        var entity = brain.Controll as Entity;
        followAbility = entity.GetAbility<PlayerFollowAbility>();
    }

    public override void OnUpdateFlow()
    {
        base.OnUpdateFlow();

        followAbility?.Move();
    }
}