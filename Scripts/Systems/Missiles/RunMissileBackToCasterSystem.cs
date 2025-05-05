using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Reflection;
using UnityEngine;

namespace Client {
    sealed class RunMissileBackToCasterSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MissileBackToCasterComponent, MissileComponent,MissilePursueComponent>> _filter;
        readonly EcsPoolInject<MissilePursueComponent> _missilePursuePool;
        readonly EcsPoolInject<MissileBackToCasterComponent> _missileBackPool;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<NextMissileComponent> _nextMissilePool;
        readonly EcsPoolInject<MissileComponent> _missilePool;
        readonly EcsPoolInject<TargetMissileComponent> _targetMissilePool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new RunMissileBackToCasterSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var transformMissileComp = ref _transformPool.Value.Get(entity);
                ref var missileBackComp = ref _missileBackPool.Value.Get(entity);
                ref var misiilePursueComp = ref _missilePursuePool.Value.Get(entity);
                ref var missileComp = ref _missilePool.Value.Get(entity);
                var positionTarget = missileComp.TargetPosition;
                positionTarget.y = 0;
                var positionMissile = transformMissileComp.Transform.position;
                positionMissile.y = 0;
                var distance = Vector3.Distance(positionTarget, positionMissile);
                if (distance < missileBackComp.DistanceDifference || misiilePursueComp.MaxSecondsPursuit <= 0)
                {
                    missileComp.TargetPosition = missileBackComp.oldTargetPosition;
                    if(missileBackComp.oldEntityTarget.Unpack(_world.Value, out int entityTarget))
                    {
                        if (!_targetMissilePool.Value.Has(entity)) _targetMissilePool.Value.Add(entity);
                        ref var targetComp = ref _targetMissilePool.Value.Get(entity);
                        targetComp.EntityTarget = missileBackComp.oldEntityTarget;
                    }
                    _missileBackPool.Value.Del(entity);
                    _missilePursuePool.Value.Del(entity);
                    _nextMissilePool.Value.Add(entity);
                }
            }
        }
    }
}