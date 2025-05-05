using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PushSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world;
        private readonly EcsFilterInject<Inc<PushEffect>,Exc<StaticUnitComponent>> _filter = default;
        private readonly EcsPoolInject<PushEffect> _pool = default;
        private readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<ExternalMoveComponent> _externalMovePool;

        public override MainEcsSystem Clone()
        {
            return new PushSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var pushEffect = ref _pool.Value.Get(entity);
                if(pushEffect.SenderEntity.Unpack(_world.Value, out int senderEntity))
                {
                    ref var transformTarget = ref _transformPool.Value.Get(entity);
                    
                    ref var transformSender = ref _transformPool.Value.Get(senderEntity);
                    var moveDirection = (transformTarget.Transform.position - transformSender.Transform.position).normalized;
                    moveDirection.y = 0;
                    if (!_externalMovePool.Value.Has(entity))
                        _externalMovePool.Value.Add(entity).Invoke(moveDirection, ForceMode.Impulse, pushEffect.PushForce, 0.1f);
                    else
                        _externalMovePool.Value.Get(entity).Invoke(moveDirection, ForceMode.Impulse, pushEffect.PushForce, 0.1f);
                }
            }
        }
    }
}