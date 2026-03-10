using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileFlowProcessor : Processor
{
    FlowRunnerAbility flowAbility;
    
    public override void Initialize()
    {
        base.Initialize();
        
        flowAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();

        var rootFlow = Flow.Create<RootFlow>(ProcessorAbility.Entity);
        rootFlow.SetProcessor(this);
        flowAbility.SetRootFlow(rootFlow);
        
        rootFlow.NextChildFlow();
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
        ProjectileEntity projectileEntity;
        IProjectileContext projectileContext;
        
        public override void OnAddFlow()
        {
            base.OnAddFlow();
            
            projectileEntity = Entity as ProjectileEntity;
            projectileContext = projectileEntity.SkillContext as IProjectileContext;
        }

        public override void OnUpdateFlow()
        {
            base.OnUpdateFlow();

            Entity.transform.position = projectileContext.NextPosition(Entity.transform.position, Time.deltaTime);
            if (elapsedTime >= projectileContext.ProjectileTime)
            {
                Parent.ActivateChildFlow<DestProjectileFlow>();
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
