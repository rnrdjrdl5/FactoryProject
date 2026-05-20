public class PlayerInputLayerProcessor : InputCommandLayerProcessor<PlayerBrainCommand>
{
    PlayerBrainCommandMapper mapper;
    PlayerBrainCommandDispatcher dispatcher;

    protected override IInputCommandMapper<PlayerBrainCommand> Mapper => mapper;
    protected override IInputCommandDispatcher<PlayerBrainCommand> Dispatcher => dispatcher;

    public override void Ready()
    {
        mapper = new PlayerBrainCommandMapper(Entity);
        dispatcher = new PlayerBrainCommandDispatcher(Entity);

        base.Ready();
    }

    public override void Uninitialize()
    {
        base.Uninitialize();

        dispatcher = null;
        mapper = null;
    }
}
