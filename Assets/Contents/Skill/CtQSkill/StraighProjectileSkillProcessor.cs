using UnityEngine;

public class StraighProjectileSkillProcessor : SkillProcessor
{
    RootFlow rootFlow;

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

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
        IStraightProjectileContext straightProjectileContext;

        public override void OnUpdateFlow()
        {
            base.OnUpdateFlow();
            
            CacheContext();

            Entity.transform.position = straightProjectileContext.NextPosition(Entity.transform.position, Time.deltaTime);
            if (elapsedTime >= straightProjectileContext.ProjectileTime)
            {
                Parent.ActivateChildFlow<DestProjectileFlow>();
            }
        }

        void CacheContext()
        {
            if (straightProjectileContext == null)
            {
                var processor = Processor as SkillProcessor;
                straightProjectileContext = processor.SkillContext as IStraightProjectileContext;
            }
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
