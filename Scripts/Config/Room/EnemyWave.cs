using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyWave", menuName = "Config/NewEnemyWave")]
public class EnemyWave : ScriptableObject
{
    public List<EnemyToSpawn> EnemiesInWave;
    public float TimerUntilNextWave = 1f;
    public int GetSumCount()
    {
        int sum = 0;
        foreach(var wave in EnemiesInWave)
        {
            sum += wave.Count;
        }
        return sum;
    }
}
[System.Serializable]
public class EnemyToSpawn
{
    public EnemyMetaDataConfig EnemyType;
    public int Count;
}