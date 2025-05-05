using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class StartTimerViewDamageSystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TakeDamageComponent, VisualAllowedComponent>> _filter = default;
        readonly EcsPoolInject<TakeDamageComponent> _takeDamagePool = default;
        readonly EcsPoolInject<ViewDamageComponent> _timerViewDamagePool = default;
        public override MainEcsSystem Clone()
        {
            return new StartTimerViewDamageSystem();
        }
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var takeDamageComp = ref _takeDamagePool.Value.Get(entity);
                if(takeDamageComp.TargetEntity.Unpack(_world.Value, out int targetEntity))
                {
                    if (!_timerViewDamagePool.Value.Has(targetEntity)) _timerViewDamagePool.Value.Add(targetEntity);
                    ref var timerComp = ref _timerViewDamagePool.Value.Get(targetEntity);
                    timerComp.Duration = 0;
                    timerComp.CurrentIntensity = 0;
                }
                
            }
        }
    }
}