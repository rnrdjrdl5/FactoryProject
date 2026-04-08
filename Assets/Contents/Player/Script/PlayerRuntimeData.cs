using Newtonsoft.Json;

public class PlayerRuntimeData : IEntityData, IMessageBus
{
    [JsonProperty] public float MaxHp { get; private set; }
    [JsonProperty] public float Hp { get; private set; }
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    public void Initialize(IInitData initData = null)
    {
    }

    public void Uninitialize()
    {
    }

    public void OnSetMessageBus()
    {
    }

    public void SetHp(float hp)
    {
        Hp = hp;
    }

    public void SetMaxHp(float maxHp)
    {
        MaxHp = maxHp;
    }

    public void FillHp()
    {
        Hp = MaxHp;
    }
}
