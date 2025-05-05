using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class ChangeMissileTargetSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ChangeMissileTargetComponent, MissileComponent, TransformComponent>> _filter;
        readonly EcsPoolInject<ChangeMissileTargetComponent> _changeMissileTargetPool;
        readonly EcsFilterInject<Inc<EnemyComponent, TransformComponent>, Exc<DeadComponent>> _filterEnemy;
        readonly EcsFilterInject<Inc<MissileComponent, TargetMissileComponent>> _filterMissileWithTarget;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<TargetMissileComponent> _targetMissilePool;
        readonly EcsPoolInject<NextMissileComponent> _nextMissilePool;
        readonly EcsWorldInject _world;
        public override MainEcsSystem Clone()
        {
            return new ChangeMissileTargetSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var changeTarget = ref _changeMissileTargetPool.Value.Get(entity);
                ref var transformMissile = ref _transformPool.Value.Get(entity); 
                var enemyDistance =  new Dictionary<float, int>();
                int targetEntity = -1;
                if(_filterEnemy.Value.GetEntitiesCount() ==0 )
                {
                    _changeMissileTargetPool.Value.Del(entity);
                    _nextMissilePool.Value.Add(entity);
                    continue;
                }
                foreach(var enemyEntity in _filterEnemy.Value)
                {
                    ref var transformEnemyComp = ref _transformPool.Value.Get(enemyEntity);
                    float distance = Vector3.Distance(transformEnemyComp.Transform.position, transformMissile.Transform.position);
                    enemyDistance.Add(distance, enemyEntity);
                }
                if(changeTarget.Range > 0)
                {
                    var range = changeTarget.Range;
                    var keysToRemove = enemyDistance.Keys.Where(distance => distance > range).ToList();

                    // Óäàëÿåì âñå íåïîäõîäÿùèå êëþ÷è
                    foreach (var key in keysToRemove)
                    {
                        enemyDistance.Remove(key);
                    }
                }
                if(_targetMissilePool.Value.Has(entity))
                {
                    ref var targetMissileCurrent = ref _targetMissilePool.Value.Get(entity);
                    if (!targetMissileCurrent.EntityTarget.Unpack(_world.Value, out int currentTarget)) continue;
                    if(enemyDistance.ContainsValue(currentTarget))
                    {
                        var key = enemyDistance.FirstOrDefault(x => x.Value == currentTarget).Key;
                        enemyDistance.Remove(key);
                    }
                }
                if(enemyDistance.Count == 0)
                {
                    _changeMissileTargetPool.Value.Del(entity);
                    _nextMissilePool.Value.Add(entity);
                }
                if(changeTarget.ÑlosestEnemy)
                {
                    var minDistance = enemyDistance.Keys.Min();
                    targetEntity = enemyDistance[minDistance];
                }
                if (changeTarget.RandomEnemy)
                {
                    if (changeTarget.UniformDistribution)
                    {
                        targetEntity = UniformDistribution(enemyDistance);
                    }
                    else
                    {
                        targetEntity = enemyDistance.Values.ElementAt(Random.Range(0, enemyDistance.Count-1));
                    }
                }
                if (targetEntity == -1) continue;
                var pakedEntityTarget = _world.Value.PackEntity(targetEntity);
                if (!_targetMissilePool.Value.Has(entity)) _targetMissilePool.Value.Add(entity);
                ref var targetCurrentMissile = ref _targetMissilePool.Value.Get(entity);
                targetCurrentMissile.EntityTarget = pakedEntityTarget;
                _changeMissileTargetPool.Value.Del(entity);
                _nextMissilePool.Value.Add(entity);
            }
        }
        public int UniformDistribution(Dictionary<float, int> enemyDistance)
        {
            var repetitionCounter = new Dictionary<int, int>(); 
            foreach(var entityEnemy in enemyDistance.Values)
            {
                repetitionCounter.Add(entityEnemy, 0);
            }
            foreach(var entityMissile in _filterMissileWithTarget.Value)
            {
                ref var targetMissileComp = ref _targetMissilePool.Value.Get(entityMissile);
                if (!targetMissileComp.EntityTarget.Unpack(_world.Value, out int entityMissileUnpack)) continue;
                if (repetitionCounter.ContainsKey(entityMissileUnpack)) repetitionCounter[entityMissileUnpack]++;
            }
            return repetitionCounter.OrderBy(kv => kv.Value).First().Key;
        }
    }
}