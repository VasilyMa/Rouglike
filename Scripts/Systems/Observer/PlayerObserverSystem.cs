using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class PlayerObserverSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<PlayerObserverComponent, PlayerComponent>> _filter = default;
        readonly EcsPoolInject<PlayerObserverComponent> _observePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;

        public override MainEcsSystem Clone()
        {
            return new PlayerObserverSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var healthComp = ref _healthPool.Value.Get(entity);
                ref var observerComp = ref _observePool.Value.Get(entity);
                observerComp.HealthValue.Value = new HealthValue(healthComp.CurrentValue, healthComp.MaxValue);
            }
        }
    }
}