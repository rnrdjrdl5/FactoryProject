using UnityEngine;

public class ProjectileEntity : Entity
{
    protected const float CollisionRadius = 0.5f;

    public SkillContext SkillContext => skillContext;
    public Tables.Projectile ProjectileData => projectileData;
    public float ElapsedTime => elapsedTime;
    public float Duration => duration;

    SkillContext skillContext;
    Tables.Projectile projectileData;
    SkillAbility skillAbility;
    float elapsedTime;
    float duration;
    int collisionCount = 1;

    protected override void PreInitialize(IInitData initData = null)
    {
        if (initData is ProjectileInitData projectileInitData)
        {
            skillContext = projectileInitData.SkillContext;
            projectileData = Tables.Projectile.Get(projectileInitData.ProjectileKey);
            var projectileParam = skillContext.SkillData.ParsedActionParam as SkillActionProjectileParam;
            duration = projectileParam.Duration.Value;
            collisionCount = projectileData.collisionCount;
        }

        base.PreInitialize(initData);
    }

    protected override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        elapsedTime = 0f;
        skillAbility = GetAbility<SkillAbility>();
        InitializeProjectile();
    }

    protected virtual void Update()
    {
        elapsedTime += Time.deltaTime;
        if (duration > 0f && elapsedTime >= duration)
        {
            OnExpire();
            return;
        }

        MoveProjectile();
    }

    protected virtual void MoveProjectile()
    {
    }

    protected virtual void InitializeProjectile()
    {
    }

    protected virtual void ProcessCollision(Entity targetEntity)
    {
        if (targetEntity == null)
        {
            return;
        }

        if (skillAbility != null && !string.IsNullOrWhiteSpace(ProjectileData?.uniqueSkillKey))
        {
            skillAbility.TryUseSkill(ProjectileData.uniqueSkillKey, SkillContext);
        }

        DecreaseCollisionCount();
        if (collisionCount <= 0)
        {
            DestroyProjectile();
        }
    }

    protected void DecreaseCollisionCount()
    {
        collisionCount--;
    }

    protected virtual void OnExpire()
    {
        DestroyProjectile();
    }

    protected void DestroyProjectile()
    {
        if (Parent == null)
        {
            return;
        }

        Parent.RemoveChild(this);
    }
}

public class ProjectileInitData : IInitData, IPositionData
{
    public string ProjectileKey { get; set; }
    public SkillContext SkillContext { get; set; }
    public Vector3 Position { get; set; }
}
