public class BaseSkillContext
{
    public Entity Caster => caster;
    public Realm Realm => realm;

    Realm realm;
    Entity caster;
    

    public void Initialize(Realm realm, Entity caster)
    {
        this.caster = caster;
        this.realm = realm;
    }
    
    public virtual void Use()
    {
        
    }

    public static SkillContextType Create<SkillContextType>(Realm realm, Entity caster) where SkillContextType : BaseSkillContext , new()
    {
        var baseSkillContext = new SkillContextType();
        baseSkillContext.Initialize(realm, caster);

        return baseSkillContext;
    }
}