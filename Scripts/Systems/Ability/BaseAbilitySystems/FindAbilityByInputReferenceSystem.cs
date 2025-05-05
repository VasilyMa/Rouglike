using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;

namespace Client {
    sealed class FindAbilityByInputReferenceSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AbilityInputEvent>> _filter = default;
        readonly EcsFilterInject<Inc<AbilityUnitComponent, PlayerComponent>> _unitFilter = default;
        readonly EcsPoolInject<AbilityInputEvent> _inputPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _unitMBPool = default;
        readonly EcsPoolInject<AbilityPressedEvent> _abilityPressedPool = default;
        readonly EcsPoolInject<AbilityReleasedEvent> _abilityReleasedPool = default;
        readonly EcsPoolInject<IsPressedAbilityComponent> _isPressedPool = default;

        public override MainEcsSystem Clone()
        {
            return new FindAbilityByInputReferenceSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var inputComp = ref _inputPool.Value.Get(entity);
                foreach(var player in _unitFilter.Value)
                {
                    ref var unitMBComp = ref _unitMBPool.Value.Get(player);
                    if(inputComp.Pressing)
                    {
                        foreach(var abilities in unitMBComp.AbilityUnitMB.AllAbilities)
                        {
                            foreach(var ability in abilities.Value)
                            {
                                if(ability.Unpack(_world.Value, out int abilityEntity))
                                {
                                    _isPressedPool.Value.Del(abilityEntity);
                                }
                            }
                        }
                    }
                    
                    if(unitMBComp.AbilityUnitMB.AllAbilities.TryGetValue(inputComp.InputAction.name, out List<EcsPackedEntity> abilityPackedEntityList))
                    {
                        foreach(var abilityPackedEntity in abilityPackedEntityList)
                        {
                            if(abilityPackedEntity.Unpack(_world.Value, out int abilityEntity))
                            {
                                if(inputComp.Pressing)
                                {
                                    _abilityPressedPool.Value.Add(abilityEntity);
                                }
                                else
                                {
                                    _abilityReleasedPool.Value.Add(abilityEntity);
                                }
                            }
                        }
                        
                    }
                }
            }
        }
    }
}