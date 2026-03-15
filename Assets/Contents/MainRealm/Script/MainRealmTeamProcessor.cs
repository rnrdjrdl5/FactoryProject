public class MainRealmTeamProcessor : Processor
{
    Team team;
    
    public override void Ready()
    {
        base.Ready();
        
        team = FactoryEntry.MainStorage.GetEntityData<Team>();
        if (team?.MessageBus != null)
        {
            team.MessageBus.Subscribe<TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
            team.MessageBus.Subscribe<TeamFormationChangedMsg>(OnTeamFormationChanged);
        }
    }

    public override void Uninitialize()
    {
        if (team != null)
        {
            if (team.MessageBus != null)
            {
                team.MessageBus.Unsubscribe<TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
                team.MessageBus.Unsubscribe<TeamFormationChangedMsg>(OnTeamFormationChanged);
            }
        }
        
        base.Uninitialize();
    }

    void OnTeamSelectedFormationChanged(TeamSelectedFormationChangedMsg msg)
    {
        if (msg.Team != team)
            return;

        CreatePlayerByTeamFormation();
    }

    void OnTeamFormationChanged(TeamFormationChangedMsg msg)
    {
        if (team == null || msg.Formation == null)
            return;

        if (msg.Formation != team.SelectedTeamFormation)
            return;

        CreatePlayerByTeamFormation();
    }

    public void CreatePlayerByTeamFormation()
    {
        var teamFormation = team.SelectedTeamFormation;
        if (teamFormation == null)
            return;

        foreach (var item in teamFormation.Players)
        {
            var playerData = Tables.Player.GetPlayerByItemKey(item.ItemKey);
            var brain = BrainLogic.CreateBrainAndEntity(Realm, Brain.PrefabPath, playerData.prefabPath);
        }
    }
}
