using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckVisualDamageSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, VisualAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<VisualAllowedComponent> _visualAllowedPool = default;
        public override MainEcsSystem Clone()
        {
            return new CheckVisualDamageSystem();
        }
        
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    //todo какая то логика по запрещению визуала 
                }
            }
        }
    }
}