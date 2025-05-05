using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ListEnemiesData", menuName = "Configs/ListEnemiesData")]
public class ListEnemiesData: ScriptableObject
{
    public List<EnemyMetaDataConfig> listMetaData = new List<EnemyMetaDataConfig>();
}
