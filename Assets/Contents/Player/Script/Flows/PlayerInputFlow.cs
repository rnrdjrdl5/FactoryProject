public class PlayerInputFlow : ProcessorFlow
{
    public override void OnAddFlow()
    {
        base.OnAddFlow();

        AddChild<NoneFlow>(Processor);
    }
}
