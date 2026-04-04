public static class PickActionLogic
{
    public static bool TryExecute(BrainActionExecutionContext context)
    {
        if (context?.PickProcessor == null)
        {
            return false;
        }

        context.PickProcessor.PickItem();
        return true;
    }
}
