public static class SkillHealLogic
{
    public static float GetHeal(PlayerFormula casterFormula, SkillActionHealParam healParam)
    {
        if (healParam?.Amount != null)
        {
            return healParam.Amount.Value;
        }

        if (!string.IsNullOrWhiteSpace(healParam?.AmountFormula))
        {
            if (casterFormula != null &&
                casterFormula.TryEvaluate(healParam.AmountFormula, out var formulaHeal))
            {
                return formulaHeal;
            }
        }

        return 0f;
    }
}
