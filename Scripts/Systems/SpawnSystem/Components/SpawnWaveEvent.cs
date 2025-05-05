using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct SpawnWaveEvent 
    {
        public EnemyWave EnemyWave;
        public Transform[] SpawnPoints;
    }
}