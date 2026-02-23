using System;
using UnityEngine;

public sealed class PlayerMoveInputElement : Element
{
    CharacterInputAbility input;
    MoveAbility moveAbility;
    float moveSpeed;
    Action<float, float> handler;

    public void Connect(CharacterInputAbility input, MoveAbility moveAbility, float speed)
    {
        this.input = input;
        this.moveAbility = moveAbility;
        moveSpeed = speed;
    }

    public override void Ready()
    {
        if (moveAbility == null)
        {
            Debug.LogWarning("MoveAbility not found.");
            return;
        }

        moveAbility.SetSpeed(moveSpeed);

        if (input == null)
        {
            Debug.LogWarning("CharacterInputAbility not found.");
            return;
        }

        handler ??= OnMoveInput;
        input.MoveInputChanged += handler;
        input.ClearInput();
    }

    public override void Uninitialize()
    {
        if (input != null && handler != null)
        {
            input.MoveInputChanged -= handler;
        }
    }

    void OnMoveInput(float x, float y)
    {
        moveAbility?.SetInput(x, y);
    }
}
