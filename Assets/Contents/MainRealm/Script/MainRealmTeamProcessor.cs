using System.Collections.Generic;
using System.Linq;

public class MainRealmTeamProcessor : Processor
{
    List<Player> players = new();
    List<Brain> brains = new();
    Team team;
    
    public override void Ready()
    {
        base.Ready();
        
        team = FactoryEntry.MainStorage.GetEntityData<Team>();
        if (team?.MessageBus != null)
        {
            team.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            team.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }
    }

    public override void Uninitialize()
    {
        if (team != null)
        {
            if (team.MessageBus != null)
            {
                team.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
                team.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
            }
        }
        
        base.Uninitialize();
    }

    void OnTeamFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (team == null || msg.Formation == null)
            return;

        if (msg.Formation != team.SelectedTeamFormation)
            return;

        CreatePlayerBySelectedTeamFormation();
    }

    void OnTeamSelectedFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
    {
        if (team == null || msg.Formation == null)
            return;
        
        CreatePlayerBySelectedTeamFormation();
    }

    public void CreatePlayerBySelectedTeamFormation()
    {
        var teamFormation = team.SelectedTeamFormation;
        if (teamFormation == null)
            return;
        
        CreatePlayerByTeamFormation(teamFormation);
    }

    public void CreatePlayerByTeamFormation(TeamFormation teamFormation)
    {
        RemovePlayerAndBrain();
        
        players.Clear();
        brains.Clear();

        Player prevPlayer = null; 
        for (int i = 0; i < teamFormation.Players.Count; i++)
        {
            var item = teamFormation.Players[i];
            var playerData = Tables.Player.GetPlayerByItemKey(item.ItemKey);
            var brain = BrainLogic.CreateBrainAndEntity(Realm, Brain.PrefabPath, playerData.prefabPath);
            
            var isAI = teamFormation.Leader != item;
            brain.SetAI(isAI);

            var player = brain.Controll as Player;
            players.Add(player);
            brains.Add(brain);

            if (prevPlayer !=null)
            {
                var followAbility = player.GetAbility<PlayerFollowAbility>();
                followAbility.SetTarget(prevPlayer);
            }

            prevPlayer = player;
        }
    }
    void RemovePlayerAndBrain()
    {
        foreach (var player in players)
        {
            Realm.RemoveChild(player);
        }

        foreach (var brain in brains)
        {
            Realm.RemoveChild(brain);
        }
    }
}
