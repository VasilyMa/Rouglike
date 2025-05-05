using UnityEngine;

namespace Client {
    struct TimerSpawnComponent {
        public float Delay;
        public UnitMetaDetail UnitConfig;
        public EnemyMetaDataConfig EnemyUnitMetaConfig;
        public Vector3 SpawnPos;
        public SourceParticle ParticleSpawn;
    }
}