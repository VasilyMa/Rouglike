using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class RunMissileToPointSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MissileComponent, MissileToPointComponent>> _filter = default;

        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<MissileToPointComponent> _pointPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<MissilePermanentComponent> _permanentPool = default;
        readonly EcsPoolInject<NextMissileComponent> _nextPool = default;

        public override MainEcsSystem Clone()
        {
            return new RunMissileToPointSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var pointComp = ref _pointPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                transformComp.Transform.position = Vector3.MoveTowards(transformComp.Transform.position, pointComp.TargetPoint, missileComp.Speed * Time.deltaTime);

                transformComp.Transform.rotation = Quaternion.LookRotation(pointComp.TargetPoint - transformComp.Transform.position);

                if (transformComp.Transform.position == pointComp.TargetPoint)
                {
                  _pointPool.Value.Del(entity);
                  _nextPool.Value.Add(entity);
                }
            }
        }
    }
}