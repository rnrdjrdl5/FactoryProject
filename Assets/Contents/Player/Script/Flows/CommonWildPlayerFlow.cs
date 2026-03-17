public class CommonWildPlayerFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();
        
        AddChild<IdleFlow>(Processor);
        AddChild<MoveFlow>(Processor);
    }
}