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
        ObjectPoolAbility objectPoolAbility;

        public override void OnEnterFlow()
        {
            base.OnEnterFlow();

            objectPoolAbility = Entity.RootAbilitySet.GetAbility<ObjectPoolAbility>();
        }

        public override void OnUpdateFlow()
        {
            base.OnUpdateFlow();
            
            CacheContext();

            Entity.transform.position = straightProjectileContext.NextPosition(Entity.transform.position, Time.deltaTime);
            if (elapsedTime >= straightProjectileContext.ProjectileTime)
            {
                Parent.ActivateChildFlow<DestProjectileFlow>();
            }

            DamageProcess();
        }

        void CacheContext()
        {
            if (straightProjectileContext == null)
            { 
                skillProcessor = Processor as SkillProcessor; 
                straightProjectileContext = skillProcessor.SkillContext as IStraightProjectileContext;
            }
        }

        void DamageProcess()
        {
            var collider = Physics2D.OverlapCircle(Entity.transform.position, straightProjectileContext.Radius, Settings.LayerId.EntityMask);
            if (collider == null || collider.gameObject == skillProcessor.SkillContext.CasterObject)
            {
                return;
            }

            var targetEntity = collider.GetComponent<Entity>();
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
