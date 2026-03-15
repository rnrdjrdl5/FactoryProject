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
    
    Team team;
    TeamFormation teamFormation;
    UITeamFormationPanelElement panelElement;
    bool isSelected;

    public void Initialize(Team team, TeamFormation teamFormation, UITeamFormationPanelElement panelElement, bool isSelected)
    {
        this.team = team;
        this.teamFormation = teamFormation;
        this.panelElement = panelElement;
        this.isSelected = isSelected;

        teamFormation.OnChanged -= RefreshUI;
        teamFormation.OnChanged += RefreshUI;
        
        RefreshUI();
    }

    void OnDisable()
    {
        teamFormation.OnChanged -= RefreshUI;
    }

    void OnDestroy()
    {
        teamFormation.OnChanged -= RefreshUI;
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

    public void OnClickRemoveFormation()
    {
        var msg = new RemoveTeamFormationMsg
        {
            TeamFormation = teamFormation,
            Team = team
        };
        
        panelElement.Panel.MessageBus.Publish(msg);
    }

    public void OnClickSelectedTeamFormation()
    {
        var msg = new SelectTeamFormationMsg
        {
            TeamFormation = teamFormation
        };
        
        panelElement.Panel.MessageBus.Publish(msg);
    }

    void OnRemoveTeamFormationItem(Item item)
    {
        var msg = new RemoveTeamFormationItemMsg
        {
            TeamFormation = teamFormation,
            Item = item
        };

        panelElement.Panel.MessageBus.Publish(msg);
    }
}

public class RemoveTeamFormationItemMsg
{
    public TeamFormation TeamFormation;
    public Item Item;
}

public class RemoveTeamFormationMsg
{
    public TeamFormation TeamFormation;
    public Team Team;
}

public class SelectTeamFormationMsg
{
    public TeamFormation TeamFormation;
}