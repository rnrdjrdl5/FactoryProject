using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainRealmPlayerEntityProcessor : Processor
{
    readonly List<Player> players = new();
    readonly List<Brain> brains = new();

    TeamStorage teamStorage;
    PlayerStorage playerStorage;
    MainRealmTeamProcessor teamProcessor;
    Team playerTeam;
    bool isSubscribedTeamStorage;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        teamProcessor = ProcessorAbility.GetProcessor<MainRealmTeamProcessor>();
    }

    public override void Ready()
    {
        base.Ready();

        TryResolveMainStorage();
    }

    public override void Uninitialize()
    {
        if (isSubscribedTeamStorage && teamStorage?.MessageBus != null)
        {
            teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
            isSubscribedTeamStorage = false;
        }

        RemovePlayerAndBrain();

        base.Uninitialize();
    }

    void OnTeamFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (teamStorage == null || msg.Formation == null)
            return;

        if (msg.Formation != teamStorage.SelectedTeamFormation)
            return;

        CreatePlayerBySelectedTeamFormation();
    }

    void OnTeamSelectedFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
    {
        if (teamStorage == null || msg.Formation == null)
            return;

        CreatePlayerBySelectedTeamFormation();
    }

    public void CreatePlayerBySelectedTeamFormation()
    {
        if (!TryResolveMainStorage())
            return;

        var teamFormation = teamStorage.SelectedTeamFormation;
        if (teamFormation == null)
            return;

        CreatePlayerByTeamFormation(teamFormation);
    }

    public void CreatePlayerByTeamFormation(TeamFormationStorage teamFormation)
    {
        if (teamFormation == null || !TryResolveMainStorage())
            return;

        RemovePlayerAndBrain();

        players.Clear();
        brains.Clear();

        var brainAbility = Realm.GetAbility<BrainAbility>();
        if (brainAbility == null)
        {
            return;
        }

        playerTeam = teamProcessor.CreateTeam(TeamType.PlayerInput, Realm);
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
            playerTeam.TryAddPlayer(player);

            var processorAbility = brain.GetAbility<BrainProcessorAbility>();
            var brainFlowProcessor = processorAbility.GetProcessor<BrainFlowProcessor>();

            if (prevPlayer != null)
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

    public Brain CreateWorldSpawnedPlayer(Spawner spawner, Team team, Vector3 position)
    {
        if (spawner == null)
        {
            return null;
        }

        var spawnedPlayerKey = spawner.SpawnerData.GetSpawnPlayerKey();
        var playerData = Tables.Player.Get(spawnedPlayerKey);
        var playerInitData = new PlayerInitData()
        {
            PlayerKey = spawnedPlayerKey,
            Position = position,
            OriginType = PlayerOriginType.WorldSpawned
        };

        var brainAbility = Realm.GetAbility<BrainAbility>();
        if (brainAbility == null)
        {
            return null;
        }

        var tuple = brainAbility.CreateBrainAndControlled<Player>(Brain.PrefabPath, playerData.prefabPath, null, playerInitData);
        var brain = tuple.brain;
        var player = tuple.controlled;
        team?.TryAddPlayer(player);

        brain.SetControlMode(BrainControlMode.AI);
        
        var brainProcessorAbility = brain.GetAbility<BrainProcessorAbility>();
        var brainFlowProcessor = brainProcessorAbility.GetProcessor<BrainFlowProcessor>();
        brainFlowProcessor.ChangeFlow<EnemyAIFlow>();
        
        return brain;
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

        teamProcessor.TryRemoveTeam(playerTeam);
        playerTeam = null;
    }

    bool TryResolveMainStorage()
    {
        teamProcessor ??= ProcessorAbility.GetProcessor<MainRealmTeamProcessor>();
        if (teamProcessor == null)
        {
            return false;
        }

        if (teamStorage != null && playerStorage != null)
        {
            TrySubscribeTeamStorage();
            return true;
        }

        var mainStorage = Entity.GetFromRoot<MainStorage>();
        if (mainStorage == null)
        {
            return false;
        }

        teamStorage ??= mainStorage.GetEntityData<TeamStorage>();
        playerStorage ??= mainStorage.GetEntityData<PlayerStorage>();
        TrySubscribeTeamStorage();

        return teamStorage != null && playerStorage != null;
    }

    void TrySubscribeTeamStorage()
    {
        if (isSubscribedTeamStorage || teamStorage?.MessageBus == null)
        {
            return;
        }

        teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
        teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        isSubscribedTeamStorage = true;
    }
}
