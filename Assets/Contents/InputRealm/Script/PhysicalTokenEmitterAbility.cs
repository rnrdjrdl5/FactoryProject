using UnityEngine;

public class PhysicalTokenEmitterAbility : Ability, IPhysicalInputTokenRequestSource
{
    PhysicalInputBindingData inputBindingData;
    IPhysicalInputTokenRequester tokenRequester;

    public void SetInputBindingData(PhysicalInputBindingData inputBindingData)
    {
        this.inputBindingData = inputBindingData;
    }

    public void SetTokenRequester(IPhysicalInputTokenRequester tokenRequester)
    {
        this.tokenRequester = tokenRequester;
    }

    public override void Uninitialize()
    {
        inputBindingData = null;
        tokenRequester = null;

        base.Uninitialize();
    }

    void Update()
    {
        if (inputBindingData == null || tokenRequester == null)
        {
            return;
        }

        foreach (var keyCode in inputBindingData.GetBoundKeyCodes())
        {
            TryPublishTokenInput(keyCode);
        }
    }

    void TryPublishTokenInput(KeyCode keyCode)
    {
        if (!Input.GetKeyDown(keyCode))
        {
            return;
        }

        if (!inputBindingData.TryGetTokenType(keyCode, out var tokenType))
        {
            return;
        }

        var tokenInput = new PhysicalInputTokenEvent
        {
            KeyCode = keyCode,
            TokenType = tokenType
        };

        tokenRequester.RequestTokenInput(tokenInput);
    }
}
