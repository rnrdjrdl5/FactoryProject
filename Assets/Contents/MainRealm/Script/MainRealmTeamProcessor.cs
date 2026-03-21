using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            var playerInitData = new PlayerInitData() { PlayerKey = playerData.Key, Position = Vector3.zero };
            var mainRealmProcessor = ProcessorAbility.GetProcessor<MainRealmProcessor>();
            var tuple = mainRealmProcessor.CreateBrainAndEntity(Realm, Brain.PrefabPath, playerData.prefabPath, null, playerInitData);
            var brain = tuple.brain;
            var player = tuple.player;

            var faction = player.GetEntityData<PlayerData>()?.Faction;
            if (faction != null)
            {
                faction.SetFactionType(Tables.FactionType.Hero);
            }
            
            players.Add(tuple.player);
            brains.Add(tuple.brain);
            
            var processorAbility = brain.GetAbility<BrainProcessorAbility>();
            var brainFlowProcessor = processorAbility.GetProcessor<BrainFlowProcessor>();

            if (prevPlayer !=null)
            {
                var followAbility = tuple.player.GetAbility<PlayerFollowAbility>();
                followAbility.SetTarget(prevPlayer);
                brainFlowProcessor.ChangeFlow<FollowCivilizedPlayerFlow>();
                brain.SetAI(true);
            }

            else
            {
                brainFlowProcessor.ChangeFlow<CommonCivilizedPlayerFlow>();
                brain.SetAI(false);
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
