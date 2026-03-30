using UnityEngine;

public class ActiveBuff
{
    string buffKey;
    Tables.Buff buffData => Tables.Buff.Get(buffKey);
    float remainDuration;
    int stack;
    float tickTimer;

    public static ActiveBuff Create(string buffKey)
    {
        var activeBuff = new ActiveBuff();
        activeBuff.Initialize(buffKey);
        return activeBuff;
    }

    public void Initialize(string buffKey)
    {
        this.buffKey = buffKey;
        remainDuration = buffData?.duration ?? 0f;
        stack = 1;
        tickTimer = 0f;
    }

    public bool Update(float deltaTime)
    {
        remainDuration -= deltaTime;
        return remainDuration <= 0f;
    }

    public void Uninitialize()
    {
    }
}
