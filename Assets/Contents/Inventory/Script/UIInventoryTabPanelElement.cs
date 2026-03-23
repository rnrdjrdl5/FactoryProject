using System;
using Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTabPanelElement : PanelElement
{
    [SerializeField] TabSlot[] tabSlots;

    void Awake()
    {
        var count = Mathf.Min(tabSlots.Length, EnumLogic.ItemTypes.Length);
        for (var i = 0; i < count; i++)
        {
            var slot = tabSlots[i];
            var itemType = EnumLogic.ItemTypes[i];
            slot.cachedAction = () => OnClickTab(itemType);
            slot.button.onClick.AddListener(slot.cachedAction);
        }
    }

    void OnDestroy()
    {
        foreach (var slot in tabSlots)
        {
            if (slot.cachedAction != null)
            {
                slot.button.onClick.RemoveListener(slot.cachedAction);
            }
        }
    }

    public override void Initialize(Panel panel, IInitData initData = null)
    {
        base.Initialize(panel, initData);

        var count = Mathf.Min(tabSlots.Length, EnumLogic.ItemTypes.Length);
        for (var i = 0; i < count; i++)
        {
            tabSlots[i].tabName.text = EnumLogic.GetItemName(EnumLogic.ItemTypes[i]);
        }
    }

    void OnClickTab(ItemType itemType)
    {
        var msg = new UIMsg.InventoryChangeTabMsg()
        {
            ItemType = itemType
        };

        Panel.MessageBus?.Publish(msg);
    }

    [Serializable]
    class TabSlot
    {
        public Button button;
        public TMP_Text tabName;
        [NonSerialized] public UnityAction cachedAction;
    }
}

public static partial class UIMsg
{
    public struct InventoryChangeTabMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public ItemType ItemType;
    }
}
