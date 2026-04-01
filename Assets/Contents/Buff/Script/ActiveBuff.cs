public class ActiveBuff
{
    BuffRunnerAbility buffRunnerAbility;
    string buffKey;
    Tables.Buff buffData => Tables.Buff.Get(buffKey);
    float remainDuration;
    int stack;
    float tickTimer;

    public static ActiveBuff Create(BuffRunnerAbility buffRunnerAbility, string buffKey)
    {
        var activeBuff = new ActiveBuff();
        activeBuff.Initialize(buffRunnerAbility, buffKey);
        return activeBuff;
    }

    public BuffRunnerAbility BuffRunnerAbility => buffRunnerAbility;
    public string BuffKey => buffKey;
    public Tables.Buff BuffData => buffData;

    public void Initialize(BuffRunnerAbility buffRunnerAbility, string buffKey)
    {
        this.buffRunnerAbility = buffRunnerAbility;
        this.buffKey = buffKey;
        remainDuration = buffData?.duration ?? 0f;
        stack = 1;
        tickTimer = 0f;

        BuffLogic.ApplyOnStart(this);
    }

    public bool Update(float deltaTime)
    {
        if (buffData != null && buffData.IsInfiniteDuration)
        {
            return false;
        }

        remainDuration -= deltaTime;
        return remainDuration <= 0f;
    }

    public void Uninitialize()
    {
        BuffLogic.ApplyOnEnd(this);
    }
}
