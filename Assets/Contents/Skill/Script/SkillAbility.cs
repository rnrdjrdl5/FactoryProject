using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility : Ability
{
    Dictionary<int, BaseSkillContext> skillContexts = new();
    Realm realm;
    ProcessorAbility processorAbility;
    
    // NOTE : 각 상황에 맞는 BaseSkillContext를 생성할 방법 생각해보기.

    public override void Initialize(Parameter parameter)
    {
        realm = Entity.GetRootParent<Realm>();
        processorAbility = Entity.GetAbility<ProcessorAbility>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var context = BaseSkillContext.Create<CtQSkillContext>(realm, Entity);
            context.ProjectileTime = 3.0f;
            context.ProjectileSpeed = 3.0f;
            context.Direction = UnityEngine.Random.insideUnitCircle;
            context.Radius = 1.0f;

            string skillKey = "CtQ";
            if (SkillFactory.TryCreateSkillProcessor(processorAbility, skillKey, out var skillProcessor))
            {
                skillProcessor.SetSkillContext(context);
            }
        }
    }
}
