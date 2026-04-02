public class ActiveBuff
{
    BuffAbility buffAbility;
    string buffKey;
    Tables.Buff buffData => Tables.Buff.Get(buffKey);

    public static ActiveBuff Create(BuffAbility buffAbility, string buffKey)
    {
        var activeBuff = new ActiveBuff();
        activeBuff.Initialize(buffAbility, buffKey);
        return activeBuff;
    }

    public BuffAbility BuffAbility => buffAbility;
    public string BuffKey => buffKey;
    public Tables.Buff BuffData => buffData;

    public void Initialize(BuffAbility buffAbility, string buffKey)
    {
        this.buffAbility = buffAbility;
        this.buffKey = buffKey;

        BuffLogic.ApplyOnStart(this);
    }

    public void Uninitialize()
    {
        BuffLogic.ApplyOnEnd(this);
    }
}
