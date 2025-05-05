using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using UI;

namespace Client {
    sealed class HealSystem : MainEcsSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<HealEvent, HealthComponent>> _filter = default;
        readonly EcsPoolInject<HealEvent> _healEvtPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;

        public override MainEcsSystem Clone()
        {
            return new HealSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var healthComp = ref _healthPool.Value.Get(entity);
                ref var healEvtComp = ref _healEvtPool.Value.Get(entity);

                healthComp.TakeHeal(healEvtComp.Heal);
            }
        }
    }
}