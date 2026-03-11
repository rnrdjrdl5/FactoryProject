using UnityEngine;

public class BrainFlowProcessor : Processor
{
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        var flowAbility = ProcessorAbility.Entity.GetAbility<FlowRunnerAbility>();

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

            AddChild<NoneFlow>(Processor);
            AddChild<AIFlow>(Processor);
        }
    }

    public class NoneFlow : ProcessorFlow
    {
        
    }

    public class AIFlow : ProcessorFlow
    {
        public override void OnAddFlow()
        {
            base.OnAddFlow();

            AddChild<IdleFlow>(Processor);
            AddChild<MoveFlow>(Processor);
        }
    }

    public class IdleFlow : ProcessorFlow
    {
        public float Duration { get; private set; } = 1;
        
        public override void OnUpdateFlow()
        {
            base.OnUpdateFlow();

            if (elapsedTime >= Duration)
            {
                parent.ActivateChildFlow<MoveFlow>();
            }
        }

        public void SetDuration(float duration)
        {
            Duration = duration;
        }
    }
    
    public class MoveFlow : ProcessorFlow
    {
        public float Duration { get; private set; } = 1;

        PlayerProcessor processor;
        Vector2 dir;

        public override void OnEnterFlow()
        {
            base.OnEnterFlow();

            var brain = Processor.Entity as Brain;
            var entity = brain.Controll as Entity;
            var processorAbility = entity.GetAbility<PlayerProcessorAbility>();
            processor = processorAbility.GetProcessor<PlayerProcessor>();
            
            dir = Random.insideUnitCircle;
        }

        public override void OnUpdateFlow()
        {
            base.OnUpdateFlow();

            if (elapsedTime >= Duration)
            {
                parent.ActivateChildFlow<IdleFlow>();
                return;
            }
            
            processor.MoveMessage(dir.x, dir.y);
        }

        public void SetDuration(float duration)
        {
            Duration = duration;
        }
    }
}
