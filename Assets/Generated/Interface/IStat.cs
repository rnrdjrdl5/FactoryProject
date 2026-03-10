using System.Collections.Generic;
using Tables;
using UnityEngine;

public interface IStats
{
    List<StatType> statTypes { get; set; }
    List<int> statValues { get; set; }
}
