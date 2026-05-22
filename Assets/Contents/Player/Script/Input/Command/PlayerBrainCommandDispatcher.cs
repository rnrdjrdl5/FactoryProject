public class PlayerBrainCommandDispatcher : IInputCommandDispatcher<PlayerBrainCommand>
{
    readonly Entity entity;
    BrainActionProcessor brainActionProcessor;

    public PlayerBrainCommandDispatcher(Entity entity)
    {
        this.entity = entity;
    }

    public LayerResult Dispatch(PlayerBrainCommand command)
    {
        return command.Type switch
        {
            PlayerBrainCommandType.Move => RequestBrainAction(new MoveBrainAction(command.Axis)),
            PlayerBrainCommandType.Pick => RequestBrainAction(new PickBrainAction()),
            PlayerBrainCommandType.UseSkill => RequestBrainAction(new UseSkillBrainAction(command.SkillKey)),
            _ => LayerResult.Pass,
        };
    }

    LayerResult RequestBrainAction<TAction>(TAction action)
        where TAction : struct, IBrainAction
    {
        RefreshBrainActionProcessor();

        return brainActionProcessor != null && brainActionProcessor.RequestAction(action)
            ? LayerResult.Consume
            : LayerResult.Pass;
    }

    void RefreshBrainActionProcessor()
    {
        if (brainActionProcessor != null)
        {
            return;
        }

        var mainRealm = entity as MainRealm ?? entity.GetParent<MainRealm>();
        if (mainRealm == null)
        {
            return;
        }

        foreach (var brain in mainRealm.GetChildren<Brain>())
        {
            if (brain == null || brain.ControlMode != BrainControlMode.PlayerInput)
            {
                continue;
            }

            var context = brain.GetProcessorContext<BrainProcessorContext>();
            if (context?.BrainActionProcessor is BrainActionProcessor playerBrainActionProcessor)
            {
                brainActionProcessor = playerBrainActionProcessor;
                return;
            }
        }
    }
}
