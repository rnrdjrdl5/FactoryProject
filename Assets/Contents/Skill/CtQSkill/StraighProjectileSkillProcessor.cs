using UnityEngine;

public class StraighProjectileSkillProcessor : SkillProcessor
{
    RootFlow rootFlow;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        rootFlow = Flow.Create<RootFlow>(ProcessorAbility.Entity);
        rootFlow.SetProcessor(this);
        rootFlow.OnAddFlow();
        rootFlow.NextChildFlow();
    }

    public override void Update()
    {
        base.Update();
        
        rootFlow?.OnUpdateFlow();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        rootFlow?.OnFixedUpdateFlow();
    }

    public class RootFlow : ProcessorFlow
    {
        public override void OnAddFlow()
        {
            base.OnAddFlow();

            AddChild<StartProjectileFlow>(Processor);
            AddChild<MoveProjectileFlow>(Processor);
            AddChild<DestProjectileFlow>(Processor);
        }
    }

    public class StartProjectileFlow : ProcessorFlow
    {
        public override void OnEnterFlow()
        {
            base.OnEnterFlow();

            parent.ActivateChildFlow<MoveProjectileFlow>();
        }
    }

    public class MoveProjectileFlow : ProcessorFlow
    {
        SkillProcessor skillProcessor;
        IStraightProjectileContext straightProjectileContext;
        Faction casterFaction;

        public override void OnEnterFlow()
        {
            base.OnEnterFlow();

            skillProcessor = Processor as SkillProcessor;
        }

        public override void OnUpdateFlow()
        {
            base.OnUpdateFlow();
            
            straightProjectileContext ??= skillProcessor.SkillContext as IStraightProjectileContext;
            Entity.transform.position = straightProjectileContext.NextPosition(Entity.transform.position, Time.deltaTime);
            
            if (elapsedTime >= straightProjectileContext.ProjectileTime)
            {
                Parent.ActivateChildFlow<DestProjectileFlow>();
            }

            DamageProcess();
        }

        void DamageProcess()
        {
            casterFaction ??= skillProcessor.SkillContext.Caster.GetEntityData<Faction>();
            
            var collider = Physics2D.OverlapCircle(Entity.transform.position, straightProjectileContext.Radius, Settings.LayerId.EntityMask);
            if (collider == null || collider.gameObject == skillProcessor.SkillContext.CasterObject)
            {
                return;
            }

            var targetEntity = collider.GetComponent<Entity>();
            var targetFaction = targetEntity.GetEntityData<Faction>();
            if (Tables.FactionRelation.GetRelation(casterFaction.FactionType, targetFaction.FactionType) !=
                Tables.FactionRelationType.Hostile)
            {
                return;
            }
            
            var hpAbility = targetEntity.GetAbility<HpAbility>();
            hpAbility.TryApplyDamage(skillProcessor.SkillContext.Caster,5);
            Realm.RemoveChild(Entity);
        }
    }

    public class DestProjectileFlow : ProcessorFlow
    {
        ObjectPoolAbility objectPoolAbility;
        
        public override void OnAddFlow()
        {
            base.OnAddFlow();

            objectPoolAbility = Entity.RootAbilitySet.GetAbility<ObjectPoolAbility>();
        }

        public override void OnEnterFlow()
        {
            base.OnEnterFlow();
            
            objectPoolAbility.DeallocateGameObject(Entity.gameObject);
        }
    }
}
