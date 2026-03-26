namespace Tables
{
    public partial class Skill
    {
        public ISkillTimeParam ParsedTimeParam
        {
            get
            {
                EnsureParsedParams();
                return parsedTimeParam;
            }
        }

        public ISkillAreaParam ParsedAreaParam
        {
            get
            {
                EnsureParsedParams();
                return parsedAreaParam;
            }
        }

        public ISkillActionParam ParsedActionParam
        {
            get
            {
                EnsureParsedParams();
                return parsedActionParam;
            }
        }

        ISkillTimeParam parsedTimeParam;
        ISkillAreaParam parsedAreaParam;
        ISkillActionParam parsedActionParam;
        bool hasParsedParams;

        public void InvalidateParsedParams()
        {
            hasParsedParams = false;
            parsedTimeParam = null;
            parsedAreaParam = null;
            parsedActionParam = null;
        }

        void EnsureParsedParams()
        {
            if (hasParsedParams)
            {
                return;
            }

            hasParsedParams = true;
            parsedTimeParam = SkillTimeParamParser.Parse(SkillTimeType, skillTimeParam, Key);
            parsedAreaParam = SkillAreaParamParser.Parse(skillAreaType, skillAreaParam, Key);
            parsedActionParam = SkillActionParamParser.Parse(skillActionType, skillActionParam, Key);
        }
    }
}
