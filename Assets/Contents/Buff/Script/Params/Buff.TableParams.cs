namespace Tables
{
    public partial class Buff
    {
        public IBuffEffectParam ParsedEffectParam => parsedEffectParam;

        IBuffEffectParam parsedEffectParam;

        public void ClearParsedParams()
        {
            parsedEffectParam = null;
        }

        public void BuildParsedParams()
        {
            ClearParsedParams();
            parsedEffectParam = BuffEffectParamParser.Parse(buffEffectType, buffEffectParam, Key);
        }
    }
}
