public class PlayerHpProcessor : Processor
{
    HpAbility hpAbility;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        hpAbility = Entity.GetAbility<HpAbility>();
        hpAbility.SetMaxHp(5);
        hpAbility.OnChangeHp += OnChangedHp;
    }

    public override void Uninitialize()
    {
        hpAbility.OnChangeHp -= OnChangedHp;
        
        base.Uninitialize();
    }
    
    void OnChangedHp(float prevHp, float hp)
    {
        if (hp <= 0)
        {
            Realm.RemoveChild(Entity);
        }
    }
}