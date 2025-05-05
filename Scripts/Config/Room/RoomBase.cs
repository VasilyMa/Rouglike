using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomBase", menuName = "Config/NewRoom")]
public class RoomBase : ScriptableObject
{
    public List<EnemyWave> enemyWaves;  
}

