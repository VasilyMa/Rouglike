using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client 
{
    sealed class TimerSpawnSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<TimerSpawnComponent>> _filter;
        readonly EcsPoolInject<TimerSpawnComponent> _timerSpawnPool;
        readonly EcsPoolInject<CreateEnemyEvent> _spawnPool;

        public override MainEcsSystem Clone()
        {
            return new TimerSpawnSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var timerComponent = ref _timerSpawnPool.Value.Get(entity);
                if (timerComponent.Delay > 0)
                {
                    timerComponent.Delay -= Time.deltaTime;
                }
                else
                {
                    ref var SpawnEvent = ref _spawnPool.Value.Add(entity);
                    SpawnEvent.SpawnPos = timerComponent.SpawnPos;
                    SpawnEvent.UnitConfig = timerComponent.UnitConfig;
                    SpawnEvent.EnemyUnitMetaConfig = timerComponent.EnemyUnitMetaConfig;
                    _timerSpawnPool.Value.Del(entity);
                }
            }
        }
    }
}