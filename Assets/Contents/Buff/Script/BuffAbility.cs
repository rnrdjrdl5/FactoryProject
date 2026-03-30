using UnityEngine;

public class BuffAbility : Ability
{
    BuffContainer buffContainer = new();

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
    }

    public void UseBuff(string buffKey)
    {
    }
}
