public class EnemyAIFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<DetectHostileTargetFlow>(Processor);
    }
}
