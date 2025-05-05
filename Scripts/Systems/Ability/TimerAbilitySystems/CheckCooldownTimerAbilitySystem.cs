using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CheckCooldownTimerAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CoolDownComponent,CooldownRecalculationComponent>, Exc<ChargePointComponent>> _filter = default;
        readonly EcsPoolInject<CoolDownComponent> _pool = default;
        readonly EcsPoolInject<CooldownRecalculationComponent> _recalculationPool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckCooldownTimerAbilitySystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var coolDownComp = ref _pool.Value.Get(entity);
                if (coolDownComp.CurrentCoolDownValue <= 0)
                    _recalculationPool.Value.Del(entity);
            }
        }
    }
}