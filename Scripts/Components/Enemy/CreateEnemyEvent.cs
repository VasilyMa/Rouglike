using UnityEngine;
namespace Client {
    struct CreateEnemyEvent {
        public UnitMetaDetail UnitConfig;
        public EnemyMetaDataConfig EnemyUnitMetaConfig;
        public Vector3 SpawnPos;
    }
}