using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckDeathBeforeDamageSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent>, Exc<ClearAllAllowedComponents>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<MomentDeadEvent> _momentDeadPool = default;
        readonly EcsPoolInject<ClearAllAllowedComponents> _clearAllowedPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckDeathBeforeDamageSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if(_deadPool.Value.Has(targetEntity) || _momentDeadPool.Value.Has(targetEntity))
                    {
                        _clearAllowedPool.Value.Add(entity);
                    }
                }
            }
        }
    }
}