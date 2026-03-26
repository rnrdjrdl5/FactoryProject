namespace Tables
{
    public partial class Skill
    {
        public ISkillTimeParam ParsedTimeParam => parsedTimeParam;
        public ISkillAreaParam ParsedAreaParam => parsedAreaParam;
        public ISkillActionParam ParsedActionParam => parsedActionParam;

        ISkillTimeParam parsedTimeParam;
        ISkillAreaParam parsedAreaParam;
        ISkillActionParam parsedActionParam;

        public void ClearParsedParams()
        {
            parsedTimeParam = null;
            parsedAreaParam = null;
            parsedActionParam = null;
        }

        public void BuildParsedParams()
        {
            ClearParsedParams();
            parsedTimeParam = SkillTimeParamParser.Parse(SkillTimeType, skillTimeParam, Key);
            parsedAreaParam = SkillAreaParamParser.Parse(skillAreaType, skillAreaParam, Key);
            parsedActionParam = SkillActionParamParser.Parse(skillActionType, skillActionParam, Key);
        }
    }
}
