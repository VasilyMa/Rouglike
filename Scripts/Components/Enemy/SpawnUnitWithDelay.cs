using UnityEngine;

namespace Client {
    struct SpawnUnitWithDelay {
        public UnitMetaDetail UnitConfig;
        public EnemyMetaDataConfig EnemyMetaConfig;
        public Vector3 SpawnPos;
        public float RandomDelay;
    }
}