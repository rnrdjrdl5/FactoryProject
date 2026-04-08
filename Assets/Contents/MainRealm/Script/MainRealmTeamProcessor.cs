using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainRealmTeamProcessor : Processor
{
    List<Player> players = new();
    List<Brain> brains = new();
    Team team;
    PlayerStorage playerStorage;
    
    public override void Ready()
    {
        base.Ready();
        
        team = FactoryEntry.MainStorage.GetEntityData<Team>();
        if (team?.MessageBus != null)
        {
            team.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            team.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }

        playerStorage = FactoryEntry.MainStorage.GetEntityData<PlayerStorage>();
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

        var brainAbility = Realm.GetAbility<BrainAbility>();
        if (brainAbility == null)
        {
            return;
        }

        Player prevPlayer = null; 
        for (int i = 0; i < teamFormation.Players.Count; i++)
        {
            var item = teamFormation.Players[i];
            var playerTableData = Tables.Player.GetPlayerByItemKey(item.ItemKey);
            
            long playerId = 0;
            if (playerStorage.TryGetPlayerIdByItemId(item.UniqueId, out var id))
            {
                playerId = id;
            }
            var playerInitData = new PlayerInitData()
            {
                PlayerKey = playerTableData.Key,
                Position = Vector3.zero,
                UniqueId = playerId,
                OriginType = PlayerOriginType.PlayerOwned
            };
            var tuple = brainAbility.CreateBrainAndControlled<Player>(Brain.PrefabPath, playerTableData.prefabPath, null, playerInitData);
            var brain = tuple.brain;
            var player = tuple.controlled;

            var faction = player.GetEntityData<PlayerData>()?.Faction;
            if (faction != null)
            {
                faction.SetFactionType(Tables.FactionType.Hero);
            }
            
            players.Add(player);
            brains.Add(brain);
            
            var processorAbility = brain.GetAbility<BrainProcessorAbility>();
            var brainFlowProcessor = processorAbility.GetProcessor<BrainFlowProcessor>();

            if (prevPlayer !=null)
            {
                var followAbility = player.GetAbility<PlayerFollowAbility>();
                followAbility.SetTarget(prevPlayer);
                brainFlowProcessor.ChangeFlow<FriendlyAIFlow>();
                brain.SetControlMode(BrainControlMode.AI);
            }

            else
            {
                brainFlowProcessor.ChangeFlow<PlayerInputFlow>();
                brain.SetControlMode(BrainControlMode.PlayerInput);
                brainAbility.SetPlayerBrain(brain);
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
            if (!Realm.GetChildren<Brain>().Contains(brain))
            {
                continue;
            }

            Realm.RemoveChild(brain);
        }
    }
}
