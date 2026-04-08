public class PlacedAIHeroFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<DetectHostileTargetFlow>(Processor);
    }
}
