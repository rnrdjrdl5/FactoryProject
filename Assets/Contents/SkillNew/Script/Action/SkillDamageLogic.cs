using UnityEngine;

public static class SkillDamageLogic
{
    public static float GetDamage(PlayerFormula casterFormula, SkillActionDamageParam damageParam)
    {
        if (damageParam?.Amount != null)
        {
            return damageParam.Amount.Value;
        }

        if (!string.IsNullOrWhiteSpace(damageParam?.AmountFormula))
        {
            if (casterFormula != null &&
                casterFormula.TryEvaluate(damageParam.AmountFormula, out var formulaDamage))
            {
                return formulaDamage;
            }
        }

        return 0f;
    }
}
