using System.Collections.Generic;

public class MainStorageSynergeProcessor : Processor
{
    public IReadOnlyList<PlayerData> TargetPlayerDatas => targetPlayerDatas;

    readonly List<PlayerData> targetPlayerDatas = new();
    PlayerStorage playerStorage;
    TeamStorage teamStorage;

    public override void Ready()
    {
        base.Ready();

        playerStorage = Entity.GetEntityData<PlayerStorage>();
        teamStorage = Entity.GetEntityData<TeamStorage>();
        if (teamStorage?.MessageBus == null)
        {
            RefreshSynerges();
            return;
        }

        teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
        teamStorage.MessageBus.Subscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnSelectedTeamFormationChanged);
        RefreshSynerges();
    }

    public override void Uninitialize()
    {
        if (teamStorage?.MessageBus != null)
        {
            teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamFormationChangedMsg>(OnTeamFormationChanged);
            teamStorage.MessageBus.Unsubscribe<EntityDataMsg.TeamSelectedFormationChangedMsg>(OnSelectedTeamFormationChanged);
        }

        targetPlayerDatas.Clear();
        playerStorage = null;
        teamStorage = null;

        base.Uninitialize();
    }

    void OnTeamFormationChanged(EntityDataMsg.TeamFormationChangedMsg msg)
    {
        if (teamStorage == null || msg.Formation == null || msg.Formation != teamStorage.SelectedTeamFormation)
        {
            return;
        }

        RefreshSynerges();
    }

    void OnSelectedTeamFormationChanged(EntityDataMsg.TeamSelectedFormationChangedMsg msg)
    {
        if (teamStorage == null)
        {
            return;
        }

        RefreshSynerges();
    }

    void RefreshSynerges()
    {
        targetPlayerDatas.Clear();

        if (teamStorage?.SelectedTeamFormation == null)
        {
            RefreshBuffs();
            return;
        }

        foreach (var item in teamStorage.SelectedTeamFormation.Players)
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
        var activeSynerges = teamStorage?.SelectedTeamFormation == null
            ? new List<ISynerge>()
            : TeamSynergeLogic.GetAllSynerges(teamStorage.SelectedTeamFormation);

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
