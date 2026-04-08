using System;
using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITeamFormationListCellView : EnhancedScrollerCellView
{
    [SerializeField] AllocGameObject cellAllocator;
    [SerializeField] GameObject selectedObject;
    [SerializeField] TMP_Text formationNameText;
    
    MessageBus externalMessageBus;
    TeamStorage teamStorage;
    TeamFormationStorage teamFormation;
    UITeamFormationPanelElement panelElement;
    bool isSelected;

    public void Initialize(TeamStorage teamStorage, TeamFormationStorage teamFormation, UITeamFormationPanelElement panelElement, bool isSelected)
    {
        Unsubscribe();

        this.teamStorage = teamStorage;
        this.teamFormation = teamFormation;
        this.panelElement = panelElement;
        this.isSelected = isSelected;
        
        externalMessageBus = panelElement.ExternalMessageBus;
        Subscribe();
        
        RefreshUI();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    void Subscribe()
    {
        if (externalMessageBus != null)
        {
            externalMessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnFormationChanged);
        }
    }
    
    void Unsubscribe()
    {
        if (externalMessageBus != null)
        {
            externalMessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnFormationChanged);
            externalMessageBus = null;
        }
    }
    
    void RefreshUI()
    {
        selectedObject.SetActive(isSelected);
        formationNameText.text = teamFormation.FormationName;
        
        var playerCount = teamFormation.Players.Count;
        cellAllocator.DeallocateObjects();
        cellAllocator.AllocateObject(playerCount);

        var index = 0;
        foreach (var item in teamFormation.Players)
        {
            var cellObject = cellAllocator.AllocatedObjects[index];
            var uiItem = cellObject.GetComponent<UIItem>();
            uiItem.UpdateItemData(item);
            uiItem.SetClickEvent(OnRemoveTeamFormationItem);

            index++;
        }
    }

    void OnFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (teamFormation == null || msg.Formation != teamFormation)
            return;

        RefreshUI();
    }

    public void OnClickPlaceTeam()
    {
        var msg = new UIMsg.PlaceTeamFormationMsg
        {
            TeamFormationStorage = teamFormation
        };

        panelElement.Panel.MessageBus.Publish(msg);
    }

    public void OnClickRemoveFormation()
    {
        var msg = new UIMsg.RemoveTeamFormationMsg
        {
            TeamFormationStorage = teamFormation,
            TeamStorage = teamStorage
        };
        
        panelElement.Panel.MessageBus.Publish(msg);
    }

    public void OnClickSelectedTeamFormation()
    {
        var msg = new UIMsg.SelectTeamFormationMsg
        {
            TeamFormationStorage = teamFormation
        };
        
        panelElement.Panel.MessageBus.Publish(msg);
    }

    public void OnClickTeamFormationGo()
    {
        var msg = new UIMsg.TeamFormationGoMsg
        {
            TeamFormationStorage = teamFormation
        };
        
        panelElement.Panel.MessageBus.Publish(msg);
    }

    void OnRemoveTeamFormationItem(Item item)
    {
        var msg = new UIMsg.RemoveTeamFormationItemMsg
        {
            TeamFormationStorage = teamFormation,
            Item = item
        };

        panelElement.Panel.MessageBus.Publish(msg);
    }
}


public static partial class UIMsg
{
    public struct RemoveTeamFormationItemMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public TeamFormationStorage TeamFormationStorage;
        public Item Item;
    }

    public struct RemoveTeamFormationMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public TeamFormationStorage TeamFormationStorage;
        public TeamStorage TeamStorage;
    }

    public struct SelectTeamFormationMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public TeamFormationStorage TeamFormationStorage;
    }

    public struct TeamFormationGoMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public TeamFormationStorage TeamFormationStorage;
    }

    public struct PlaceTeamFormationMsg : IMessageOrigin
    {
        public MessageOriginType Origin => MessageOriginType.UI;
        public TeamFormationStorage TeamFormationStorage;
    }
}
