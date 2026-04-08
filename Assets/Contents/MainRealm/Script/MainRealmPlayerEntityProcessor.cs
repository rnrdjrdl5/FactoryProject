using UnityEngine;

public class MainRealmPlayerEntityProcessor : Processor
{
    TeamStorage teamStorage;
    PlayerStorage playerStorage;
    MainRealmTeamProcessor teamProcessor;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);

        teamProcessor = ProcessorAbility.GetProcessor<MainRealmTeamProcessor>();
    }

    public override void Ready()
    {
        base.Ready();

        SetStorage();
    }

    public override void Uninitialize()
    {
        if (teamStorage?.MessageBus != null)
        {
            teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
        }

        RemoveHeroPlayerAndBrain();
        RemovePlacedAIHeroPlayerAndBrain();

        base.Uninitialize();
    }

    void OnTeamFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (teamStorage == null || msg.Formation == null)
            return;

        if (msg.Formation != teamStorage.SelectedTeamFormation)
            return;

        CreateControlledHeroTeam();
    }

    void OnTeamSelectedFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
    {
        if (teamStorage == null || msg.Formation == null)
            return;

        CreateControlledHeroTeam();
    }

    public void CreateControlledHeroTeam()
    {
        if (teamStorage == null)
            return;

        var teamFormation = teamStorage.SelectedTeamFormation;
        if (teamFormation == null)
            return;

        CreateControlledHeroTeam(teamFormation);
    }

    public void CreateControlledHeroTeam(TeamFormationStorage teamFormation)
    {
        if (teamFormation == null || playerStorage == null || teamProcessor == null)
            return;

        RemoveHeroPlayerAndBrain();

        var brainAbility = Realm.GetAbility<BrainAbility>();
        if (brainAbility == null)
        {
            return;
        }

        var controlledTeam = teamProcessor.CreateControlledTeam(Realm);
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

            controlledTeam.TryAddPlayer(player);

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
                brainAbility.SetMainPlayerBrain(brain);
            }

            prevPlayer = player;
        }
    }

    public bool PlaceAIHeroTeam(TeamFormationStorage teamFormation)
    {
        if (teamFormation == null || playerStorage == null || teamProcessor == null)
        {
            return false;
        }

        if (teamProcessor.TryGetTeam(TeamType.PlayerAI, teamFormation.UniqueId, out var placedTeam))
        {
            RemovePlacedAIHeroTeam(placedTeam);
            return true;
        }

        var brainAbility = Realm.GetAbility<BrainAbility>();
        var controlledPlayer = brainAbility?.MainPlayerBrain?.Controll as Player;
        if (controlledPlayer == null)
        {
            return false;
        }

        var aiTeam = teamProcessor.CreateTeam(TeamType.PlayerAI, Realm, teamFormation.UniqueId);

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
                Position = controlledPlayer.transform.position,
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

            aiTeam.TryAddPlayer(player);

            brain.SetControlMode(BrainControlMode.AI);

            var processorAbility = brain.GetAbility<BrainProcessorAbility>();
            var brainFlowProcessor = processorAbility.GetProcessor<BrainFlowProcessor>();
            brainFlowProcessor.ChangeFlow<PlacedAIHeroFlow>();
        }

        return true;
    }

    public Brain CreatePlayerBySpawner(Spawner spawner, Team team, Vector3 position)
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

    void RemoveHeroPlayerAndBrain()
    {
        var controlledTeam = teamProcessor.ControlledTeam;
        if (controlledTeam == null)
        {
            return;
        }

        for (int i = controlledTeam.Players.Count - 1; i >= 0; i--)
        {
            Realm.RemoveChild(controlledTeam.Players[i]);
        }

        teamProcessor.TryRemoveControlledTeam();
    }

    void RemovePlacedAIHeroPlayerAndBrain()
    {
        for (int i = teamProcessor.Teams.Count - 1; i >= 0; i--)
        {
            var team = teamProcessor.Teams[i];
            if (team.TeamType != TeamType.PlayerAI || team.TeamFormationUniqueId == 0)
            {
                continue;
            }

            RemovePlacedAIHeroTeam(team);
        }
    }

    void RemovePlacedAIHeroTeam(Team team)
    {
        for (int i = team.Players.Count - 1; i >= 0; i--)
        {
            Realm.RemoveChild(team.Players[i]);
        }

        teamProcessor.TryRemoveTeam(team);
    }

    bool SetStorage()
    {
        teamProcessor ??= ProcessorAbility.GetProcessor<MainRealmTeamProcessor>();
        if (teamProcessor == null)
        {
            return false;
        }

        if (teamStorage != null && playerStorage != null)
        {
            SetTeamStorage();
            return true;
        }

        var mainStorage = Entity.GetFromRoot<MainStorage>();
        if (mainStorage == null)
        {
            return false;
        }

        teamStorage ??= mainStorage.GetEntityData<TeamStorage>();
        playerStorage ??= mainStorage.GetEntityData<PlayerStorage>();
        SetTeamStorage();

        return teamStorage != null && playerStorage != null;
    }

    void SetTeamStorage()
    {
        if (teamStorage?.MessageBus == null)
        {
            return;
        }

        teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
        teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnTeamSelectedFormationChanged);
    }
}
