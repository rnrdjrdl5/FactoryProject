public class PlayerAnimatorEventDispatcher : IAnimatorEventDispatcher
{
    readonly Player player;
    readonly MessageBus messageBus;

    public PlayerAnimatorEventDispatcher(Player player)
    {
        this.player = player;
        messageBus = player?.MessageBus;
    }

    public void Dispatch(AnimationEventReceiver sender, string eventKey)
    {
        if (player == null || messageBus == null)
        {
            return;
        }

        switch (eventKey)
        {
            case PlayerAnimationEventKeys.AttackFire:
                messageBus.Publish(new PlayerAnimationMsg.AttackFireMsg
                {
                    Player = player
                });
                break;
            case PlayerAnimationEventKeys.ComboOpen:
                messageBus.Publish(new PlayerAnimationMsg.ComboOpenMsg
                {
                    Player = player
                });
                break;
            case PlayerAnimationEventKeys.ComboClose:
                messageBus.Publish(new PlayerAnimationMsg.ComboCloseMsg
                {
                    Player = player
                });
                break;
        }
    }
}
