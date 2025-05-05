using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    /// <summary>
    /// A system that spawns enemies,
    /// Yes, there is a lot of nesting, but there is nothing you can do about it
    /// </summary>
    sealed class SpawnWaveSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<SpawnWaveEvent>> _filter;
        readonly EcsPoolInject<SpawnWaveEvent> _spawnWavePool;
        readonly EcsWorldInject _world;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<SpawnUnitWithDelay> _spawnWithDelayPool = default;

        public override MainEcsSystem Clone()
        {
            return new SpawnWaveSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var waveComp = ref _spawnWavePool.Value.Get(entity);
                foreach(var enemyData in waveComp.EnemyWave.EnemiesInWave) 
                {
                    for (int i = 0; i < enemyData.Count; i++)
                    {
                        int createUnit = _world.Value.NewEntity();
                        var UnitConfig = enemyData.EnemyType.GetUnit();
                            //ConfigModule.GetConfig<UnitConfig>().GetTierConfigByTierLevel(enemyData.EnemyType).GetRandomUnit();

                        ref var spawnWithDelayEvent = ref _spawnWithDelayPool.Value.Add(createUnit);
                        spawnWithDelayEvent.UnitConfig = UnitConfig;
                        spawnWithDelayEvent.EnemyMetaConfig = enemyData.EnemyType;
                        spawnWithDelayEvent.SpawnPos = waveComp.SpawnPoints[Random.Range(0, waveComp.SpawnPoints.Length)].position;
                        spawnWithDelayEvent.RandomDelay = Random.Range(0,4) * 0.2f;
                    }
                }
            }
        }
    }
}