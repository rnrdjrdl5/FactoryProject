using UnityEngine;

public sealed class PlayerDirector : Director
{
    [SerializeField] float moveSpeed = 5f;

    PlayerMoveInputElement moveInputElement;

    protected override void BuildElements()
    {
        moveInputElement = new PlayerMoveInputElement();
        AddElement(moveInputElement);
    }

    protected override void WireElements()
    {
        if (Host == null)
        {
            Debug.LogWarning("PlayerDirector host(Entity) not found.", this);
            return;
        }

        var inputAbility = Host.AddAbility<CharacterInputAbility>();
        var moveAbility = Host.AddAbility<MoveAbility>();

        moveInputElement?.Connect(inputAbility, moveAbility, moveSpeed);
    }
}
