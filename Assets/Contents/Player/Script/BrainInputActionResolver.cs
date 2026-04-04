using UnityEngine;

public class BrainInputActionResolver
{
    public bool TryResolve(BrainActionRequest request, BrainActionExecutionContext context)
    {
        switch (request.InputActionType)
        {
            case InputActionType.Move:
                return MoveActionLogic.TryExecute(context, request.Direction);
            case InputActionType.Pick:
                return PickActionLogic.TryExecute(context);
            case InputActionType.MainAttack:
            case InputActionType.SubAttack:
            case InputActionType.Skill1:
            case InputActionType.Skill2:
            case InputActionType.Skill3:
            case InputActionType.MainUtility:
            case InputActionType.SubUtility:
                return UseSkillActionLogic.TryExecuteByInputAction(context, request.InputActionType);
            default:
                return false;
        }
    }
}
