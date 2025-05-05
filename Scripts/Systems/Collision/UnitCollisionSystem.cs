using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    sealed class UnitCollisionSystem : MainEcsSystem {

        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<UnitCollisionEvent>> _filter = default;
        readonly EcsPoolInject<UnitCollisionEvent> _pool = default;
        readonly EcsPoolInject<DoResolveBlockEvent> _doResolvePool = default;
        readonly EcsPoolInject<ResolveBlockComponent> _resolveBlockPool = default;

        public override MainEcsSystem Clone()
        {
            return new UnitCollisionSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var unitCollisionComp = ref _pool.Value.Get(entity);
                foreach(var collisionPackedEntity in unitCollisionComp.CollisionEntity)
                {
                    if(collisionPackedEntity.Unpack(_world.Value, out int collisionEntity))
                    {
                        if (!_doResolvePool.Value.Has(collisionEntity)) _doResolvePool.Value.Add(collisionEntity);
                        ref var doResolveComp = ref _doResolvePool.Value.Get(collisionEntity);
                        ref var resolveBlockComp = ref _resolveBlockPool.Value.Get(entity);
                        doResolveComp.Components = new (resolveBlockComp.Components);
                        doResolveComp.SenderPackedEntity = unitCollisionComp.SenderPackedEntity;
                    }
                }
            }
        }
    }
}