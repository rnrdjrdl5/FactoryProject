using UnityEngine;

public class SkillProcessor : UpdateProcessor
{
    public BaseSkillContext SkillContext => skillContext;
    
    BaseSkillContext skillContext;
    System.Action OnSetSkillContext;
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        OnSetSkillContext += OnReadySkillContext;
    }

    public override void Uninitialize()
    {
        OnSetSkillContext -= OnReadySkillContext;
        
        base.Uninitialize();
    }

    public virtual void OnReadySkillContext()
    {
        
    }

    public SkillContextType GetSkillContext<SkillContextType>() where SkillContextType : BaseSkillContext
    {
        return skillContext as SkillContextType;
    }

    public void SetSkillContext(BaseSkillContext skillContext)
    {
        this.skillContext = skillContext;
        OnSetSkillContext?.Invoke();
    }
}
