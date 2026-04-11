using UnityEngine;

public class PlayerModelProcessor : Processor
{
    public AttackType AttackType => attackType;
    public PixemAnimationType StateType => stateType;

    Player player;
    PixemRuntimeCharacter character;
    AttackType attackType;
    PixemAnimationType stateType;
    bool isFlip;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        player = Entity as Player;
        character = player.View.Character;

        InitStateType();
        InitSkin();
    }

    void InitSkin()
    {
        var skinData = Tables.Skin.Get(player.TableData.skinKey);
        
        character.EquipPart(PixemPartType.Body, skinData.bodyKey);
        character.EquipPart(PixemPartType.FaceAcc1, skinData.eyeKey);
        character.EquipPart(PixemPartType.Hair, skinData.hairKey);
    }
    
    void InitStateType()
    {
        SetAttackType(AttackType.None);
        SetStateType(PixemAnimationType.Idle);
    }

    public void SetAttackType(AttackType attackType)
    {
        this.attackType = attackType;
        RefreshAnimation();
    }

    public void SetStateType(PixemAnimationType stateType)
    {
        this.stateType = stateType;
        RefreshAnimation();
    }

    public void SetFlip(bool isFlip)
    {
        this.isFlip = isFlip;
        RefreshDirection();
    }
    
    void RefreshAnimation()
    {
        character.Animator.SetInteger(AnimatorParameters.AttackType, (int)attackType);
        character.Animator.PlayAnimation((int)stateType);
        
        if (stateType == PixemAnimationType.Death)
        {
            SetAnyStateTrigger();
        }
    }

    void RefreshDirection()
    {
        var xScale = isFlip ? -1.0f : 1.0f;
        character.ModelObject.transform.localScale = character.ModelObject.transform.localScale.SetX(xScale);
    }

    public void EquipModel(PixemPartType partType, string key)
    {
        character.EquipPart(partType, key);
    }

    public void EquipModel(PixemPartType partType, Tables.Item itemData)
    {
        EquipModel(partType, itemData.equipPath);
    }
    
    void SetAnyStateTrigger()
    
    {
        character.Animator.SetTrigger(AnimatorParameters.AnyStateTrigger);
    }
    
    public void PlayAttack()
    {
        character.Animator.SetTrigger(AnimatorParameters.AttackTrigger);
    }
    
    static class AnimatorParameters
    {
        static int GetHashCode(string parameter) => Animator.StringToHash(parameter);
        public static int AttackType = GetHashCode(nameof(AttackType));
        public static int AttackTrigger = GetHashCode(nameof(AttackTrigger));
        public static int AnyStateTrigger = GetHashCode(nameof(AnyStateTrigger));
    }
}
