public class MainRealmTeamProcessor : Processor
{
    Team team;
    
    public override void Ready()
    {
        base.Ready();
        
        team = FactoryEntry.MainStorage.GetEntityData<Team>();
        team.OnChanged += OnChanged;
    }

    public override void Uninitialize()
    {
        if (team != null)
        {
            team.OnChanged -= OnChanged;
        }
        
        base.Uninitialize();
    }

    void OnChanged()
    {

    }

    public void CreatePlayerByTeamFormation()
    {
        var teamFormation = team.SelectedTeamFormation;

        foreach (var item in teamFormation.Players)
        {
            var playerData = Tables.Player.GetPlayerByItemKey(item.ItemKey);
            var brain = BrainLogic.CreateBrainAndEntity(Realm, Brain.PrefabPath, playerData.prefabPath);
        }
    }
}