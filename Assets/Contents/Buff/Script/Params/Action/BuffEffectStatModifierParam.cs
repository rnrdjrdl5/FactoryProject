public class BuffEffectStatModifierParam : IBuffEffectParam
{
    public Tables.StatType? StatType { get; set; }
    public int? Value { get; set; }

    public StatModifier CreateStatModifier()
    {
        if (StatType == null || Value == null)
        {
            return null;
        }

        var statModifier = new StatModifier();
        statModifier.statTypes.Add(StatType.Value);
        statModifier.statValues.Add(Value.Value);
        return statModifier;
    }
}
