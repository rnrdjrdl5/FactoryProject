using UnityEngine;

public class ProjectileEntity : Entity
{
    public SkillContext SkillContext => skillContext;

    SkillContext skillContext;

    protected override void Initialize(IInitData initData = null)
    {
        if (initData is ProjectileInitData projectileInitData)
        {
            skillContext = projectileInitData.SkillContext;
        }

        base.Initialize(initData);
    }
}

public class ProjectileInitData : IInitData, IPositionData
{
    public SkillContext SkillContext { get; set; }
    public Vector3 Position { get; set; }
}
