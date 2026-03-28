using UnityEngine;
using System.Collections.Generic;

public class SkillContext
{
    public Entity Caster { get; set; }
    public Tables.Skill SkillData { get; set; }
    public Vector3? TargetPosition { get; set; }
    public List<Entity> TargetEntities { get; set; }
}
