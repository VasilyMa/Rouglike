using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Client
{
    /// <summary>
    /// A system for delaying spawn of enemies
    /// </summary>
    sealed class ParticleSpawnEnemySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<SpawnUnitWithDelay>> _filter;
        readonly EcsSharedInject<GameState> _state;
        readonly EcsPoolInject<SpawnUnitWithDelay> _delayPool;
        readonly EcsPoolInject<TimerSpawnComponent> _timerSpawnPool;

        public override MainEcsSystem Clone()
        {
            return new ParticleSpawnEnemySystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var delayComp = ref _delayPool.Value.Get(entity);
                delayComp.RandomDelay -= Time.deltaTime;
                if (delayComp.RandomDelay >= 0) continue;
                ref var timerComp = ref _timerSpawnPool.Value.Add(entity);
                timerComp.SpawnPos = delayComp.SpawnPos + new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2));
                timerComp.UnitConfig = delayComp.UnitConfig;
                timerComp.EnemyUnitMetaConfig = delayComp.EnemyMetaConfig;
                timerComp.Delay = delayComp.UnitConfig.SpawnDelay;
                if (timerComp.UnitConfig.ParticleSpawn != null)
                {
                    var particle = PoolModule.Instance.GetFromPool<SourceParticle>(timerComp.UnitConfig.ParticleSpawn, true);

                    particle.transform.position = delayComp.SpawnPos;
                    particle.transform.rotation = Quaternion.identity;
                    timerComp.ParticleSpawn = particle;
                    timerComp.ParticleSpawn.Invoke();
                    
                }
                _delayPool.Value.Del(entity);
            }
        }
    }
}