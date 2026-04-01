public class ActiveBuff
{
    BuffAbility buffAbility;
    string buffKey;
    Tables.Buff buffData => Tables.Buff.Get(buffKey);
    float remainDuration;
    int stack;
    float tickTimer;

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
        remainDuration = buffData?.duration ?? 0f;
        stack = 1;
        tickTimer = 0f;

        BuffLogic.ApplyOnStart(this);
    }

    public bool Update(float deltaTime)
    {
        remainDuration -= deltaTime;
        return remainDuration <= 0f;
    }

    public void Uninitialize()
    {
        BuffLogic.ApplyOnEnd(this);
    }
}
