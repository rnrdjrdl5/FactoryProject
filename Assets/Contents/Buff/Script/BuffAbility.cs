public class BuffAbility : Ability
{
    BuffContainer buffContainer = new();

    public void UseBuff(string buffKey)
    {
        // TODO: define duplicate/reapply policy when the same buffKey already exists at runtime.
        buffContainer.AddBuff(this, buffKey);
    }

    public void RemoveBuff(string buffKey)
    {
        buffContainer.RemoveBuff(buffKey);
    }
}
