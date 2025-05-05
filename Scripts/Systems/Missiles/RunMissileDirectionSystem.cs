using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class RunMissileDirectionSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<MissileComponent, MissileDirectionComponent>> _filter = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<MissileDirectionComponent> _directionPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<NextMissileComponent> _nextPool = default;

        public override MainEcsSystem Clone()
        {
            return new RunMissileDirectionSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var directionComp = ref _directionPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);
                transformComp.Transform.position += directionComp.Direction * Time.deltaTime * missileComp.Speed;
                transformComp.Transform.forward = directionComp.Direction.normalized;

                directionComp.PassedWay += Time.deltaTime * missileComp.Speed;

                if (directionComp.PassedWay >= directionComp.Distance)
                {
                    _directionPool.Value.Del(entity);
                    _nextPool.Value.Add(entity);
                }
            }
        }
    }
}