using UnityEngine;
using System.Collections.Generic;

public class SkillContext
{
    public SkillContext ParentContext { get; set; }
    public Entity OriginCaster { get; set; }
    public Entity Caster { get; set; }
    public Tables.Skill SkillData { get; set; }
    public Vector3? TargetPosition { get; set; }
    public List<Entity> TargetEntities { get; set; }

    public static SkillContext Create(
        SkillContext parentContext,
        Entity originCaster,
        Entity caster,
        Tables.Skill skillData,
        Vector3? targetPosition = null,
        List<Entity> targetEntities = null)
    {
        return new SkillContext
        {
            ParentContext = parentContext,
            OriginCaster = originCaster,
            Caster = caster,
            SkillData = skillData,
            TargetPosition = targetPosition,
            TargetEntities = targetEntities
        };
    }
}
