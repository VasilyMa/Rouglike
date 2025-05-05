using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AbilityPressedSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AbilityPressedEvent, AbilityComponent>, Exc<CheckAbilityToUse>> _filter = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<CheckAbilityToUse> _checkAbilityToUsePool = default;
        readonly EcsPoolInject<IsPressedAbilityComponent> _isPressedPool = default;

        public override MainEcsSystem Clone()
        {
            return new AbilityPressedSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                foreach(var inputBlock in abilityComp.Ability.SourceAbility.InputBlocks)
                {
                    if(inputBlock.Pressing)
                    {
                        _checkAbilityToUsePool.Value.Add(entity);
                        if(!_isPressedPool.Value.Has(entity)) _isPressedPool.Value.Add(entity);
                    }
                }
            }
        }
    }
}