using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;

namespace Client
{
    sealed class CoerciveAbilitySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<CoerciveAbilityComponent>, Exc<InActionComponent>> _filter;
        readonly EcsPoolInject<CoerciveAbilityComponent> _coerciveAbilityPool;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;

        public override MainEcsSystem Clone()
        {
            return new CoerciveAbilitySystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var coerciveAbilityComp = ref _coerciveAbilityPool.Value.Get(entity);
                if(!_abilityPressedPool.Value.Has(coerciveAbilityComp.EntityAbility)) 
                    _abilityPressedPool.Value.Add(coerciveAbilityComp.EntityAbility);
                _coerciveAbilityPool.Value.Del(entity);
            }
        }
    }
}
