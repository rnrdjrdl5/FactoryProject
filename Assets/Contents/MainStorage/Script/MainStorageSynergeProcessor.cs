using System.Collections.Generic;

public class MainStorageSynergeProcessor : Processor
{
    public IReadOnlyList<PlayerData> TargetPlayerDatas => targetPlayerDatas;

    readonly List<PlayerData> targetPlayerDatas = new();
    PlayerStorage playerStorage;
    Team team;

    public override void Ready()
    {
        base.Ready();

        playerStorage = Entity.GetEntityData<PlayerStorage>();
        team = Entity.GetEntityData<Team>();
        if (team?.MessageBus == null)
        {
            RefreshSynerges();
            return;
        }

        team.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
        team.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnSelectedTeamFormationChanged);
        RefreshSynerges();
    }

    public override void Uninitialize()
    {
        if (team?.MessageBus != null)
        {
            team.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            team.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnSelectedTeamFormationChanged);
        }

        targetPlayerDatas.Clear();
        playerStorage = null;
        team = null;

        base.Uninitialize();
    }

    void OnTeamFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (team == null || msg.Formation == null || msg.Formation != team.SelectedTeamFormation)
        {
            return;
        }

        RefreshSynerges();
    }

    void OnSelectedTeamFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
    {
        if (team == null)
        {
            return;
        }

        RefreshSynerges();
    }

    void RefreshSynerges()
    {
        targetPlayerDatas.Clear();

        if (team?.SelectedTeamFormation == null)
        {
            RefreshBuffs();
            return;
        }

        foreach (var item in team.SelectedTeamFormation.Players)
        {
            if (item == null)
            {
                continue;
            }

            if (!playerStorage.TryGetPlayerDataByItemUid(item.UniqueId, out var playerData) || playerData == null)
            {
                continue;
            }

            targetPlayerDatas.Add(playerData);
        }

        RefreshBuffs();
    }

    void RefreshBuffs()
    {
        if (playerStorage == null)
        {
            return;
        }

        var allSynerges = TeamSynergeLogic.GetAllSynerges();
        var activeSynerges = team?.SelectedTeamFormation == null
            ? new List<ISynerge>()
            : TeamSynergeLogic.GetAllSynerges(team.SelectedTeamFormation);

        foreach (var playerData in playerStorage.PlayerDataByKey.Values)
        {
            if (playerData?.Buff == null)
            {
                continue;
            }

            foreach (var synerge in allSynerges)
            {
                if (string.IsNullOrWhiteSpace(synerge.synergeBuffKey))
                {
                    continue;
                }

                playerData.Buff.RemoveBuff(synerge.synergeBuffKey);
            }

            if (!targetPlayerDatas.Contains(playerData))
            {
                continue;
            }

            foreach (var synerge in activeSynerges)
            {
                if (string.IsNullOrWhiteSpace(synerge.synergeBuffKey))
                {
                    continue;
                }

                playerData.Buff.AddBuff(synerge.synergeBuffKey, synerge.Key, BuffLifetimeType.External);
            }
        }
    }
}
