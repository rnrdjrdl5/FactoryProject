namespace Tables
{
    public partial class Buff : IIconSprite
    {
        public Skill StartSkill => Skill.Get(startSkillKey);
        public Skill TickSkill => Skill.Get(tickSkillKey);
        public Skill EndSkill => Skill.Get(endSkillKey);
    }
}
