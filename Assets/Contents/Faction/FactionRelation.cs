using System;

public class FactionRelation : IEntityData
{
    FactionRelationType[,] relation;
    
    public void Initialize(IInitData initData = null)
    {
        var count = Enum.GetValues(typeof(FactionType)).Length;
        relation = new FactionRelationType[count, count];
        for (var i = 0; i < count; i++)
        {
            for (var j = 0; j < count; j++)
            {
                relation[i, j] = i == j ? FactionRelationType.Friendly : FactionRelationType.Neutral;
            }
        }
    }

    public void Uninitialize()
    {
        relation = null;
    }

    public FactionRelationType GetRelation(FactionType from, FactionType to)
    {
        return relation[(int)from, (int)to];
    }

    public void SetRelation(FactionType from, FactionType to, FactionRelationType value)
    {
        relation[(int)from, (int)to] = value;
    }
}
