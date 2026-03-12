using UnityEngine;

public class PlayerHpProcessor : Processor
{
    Player player;
    HpAbility hpAbility;
    DropItemProcessor dropItemProcessor;
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        player = Entity as Player;
        
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
            dropItemProcessor ??= ProcessorAbility.GetProcessor<DropItemProcessor>();
            dropItemProcessor.TryDropItem(Entity.transform.position, player.PlayerData.dropPlayerPercent, player.PlayerData.dropPlayerKey);
            
            DestroyPlayer();
        }
    }
    
    void DestroyPlayer()
    {
        Realm.RemoveChild(Entity);
    }
}