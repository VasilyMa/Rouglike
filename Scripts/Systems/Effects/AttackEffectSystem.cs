using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client {
    sealed class AttackEffectSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AbilityComponent, InitTimerAbilityEvent, AttackEffectComponent>, Exc<TimerAbilityComponent>> _filter = default;

        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<AttackEffectComponent> _attackEffectPool = default;

        public override MainEcsSystem Clone()
        {
            return new AttackEffectSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);

                if (ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    ref var attackEffectComp = ref _attackEffectPool.Value.Get(entity);
                    attackEffectComp.Invoke(ownerEntity, entity, _world.Value);
                }
            }
        }
    }
}