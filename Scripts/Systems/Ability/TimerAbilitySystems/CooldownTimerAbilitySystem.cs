using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class CooldownTimerAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CooldownRecalculationComponent, CoolDownComponent>> _filter = default;
        readonly EcsPoolInject<CoolDownComponent> _pool = default;

        public override MainEcsSystem Clone()
        {
            return new CooldownTimerAbilitySystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var coolDownComp = ref _pool.Value.Get(entity);
                coolDownComp.CurrentCoolDownValue -= Time.deltaTime;
                if (coolDownComp.CurrentCoolDownValue < 0)
                    coolDownComp.CurrentCoolDownValue = 0;

                coolDownComp.OnCooldown?.Invoke(coolDownComp.CurrentCoolDownValue, coolDownComp.CoolDownValue);
            }
        }
    }
}