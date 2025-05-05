using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitEnemyPoolSystem : MainEcsSystem
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<SpawnUnitWithDelay>> _filter = default;
        readonly EcsPoolInject<SpawnUnitWithDelay> _filterPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitEnemyPoolSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var createEnemyEvent = ref _filterPool.Value.Get(entity);

                string EnemyPoolName = createEnemyEvent.UnitConfig.Unit.name;
                string EnemySpawnParticlePoolName = createEnemyEvent.UnitConfig.ParticleSpawn.name;
/*
                if (!_state.Value.TryGetPool(createEnemyEvent.UnitConfig.Unit, out var pool))
                {
                    _state.Value.CreatePool(createEnemyEvent.UnitConfig.Unit,EnemyPoolName);
                }
                if (!_state.Value.TryGetPool(createEnemyEvent.UnitConfig.ParticleSpawn.gameObject, out var pool2))
                {
                    _state.Value.CreatePool(createEnemyEvent.UnitConfig.ParticleSpawn.gameObject,EnemySpawnParticlePoolName);
                }*/
            }
        }
    }
}