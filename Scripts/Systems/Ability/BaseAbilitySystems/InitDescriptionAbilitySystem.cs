using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class InitDescriptionAbilitySystem : MainEcsSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InitAbilityEvent, AbilityComponent>> _filter = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<InstantComponent> _instantAbilityPool = default;
        readonly EcsPoolInject<AttackAbility> _attackAbilityPool = default;
        readonly EcsPoolInject<DefenceAbility> _defenceAbilityPool = default;
        readonly EcsPoolInject<UtilityAbility> _utilityAbilityPool = default;
        readonly EcsPoolInject<TerrorizeAbility> _terrorizeAbilityPool = default;
        readonly EcsPoolInject<InputReferenceComponent> _inputReferencePool = default;
        readonly EcsPoolInject<TimerStartAtPressedComponent> _pressedPool = default;
        readonly EcsPoolInject<TimerStartAtReleasedComponent> _releasedPool = default;
        readonly EcsPoolInject<PreRequisiteComponent> _preRequisitePool = default;
        readonly EcsPoolInject<ChargeComponent> _chargePool = default;
        readonly EcsPoolInject<ChargeIndicatorComponent> _chargeIndicatorPool = default;
        readonly EcsPoolInject<NonWaitClickable> _nonWaitPool = default;
        readonly EcsPoolInject<InitAbilityEvent> _initEventPool = default;

        public override MainEcsSystem Clone()
        {
            return new InitDescriptionAbilitySystem();
        }

        readonly EcsPoolInject<PlayerAbilityComponent> _playerAbilityPool = default;
        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityPool.Value.Get(entity);
                ref var ability = ref abilityComp.Ability.SourceAbility;
                ref var initEvent = ref _initEventPool.Value.Get(entity);
                if (ability.InstantAbility)
                {
                    _instantAbilityPool.Value.Add(entity);
                    _nonWaitPool.Value.Add(entity);
                }
                switch (ability.AbilityType)
                {
                    case AbilitySystem.AbilityTypes.Attack:
                        _attackAbilityPool.Value.Add(entity);
                        break;
                    case AbilitySystem.AbilityTypes.Defence:
                        _defenceAbilityPool.Value.Add(entity);
                        break;
                    case AbilitySystem.AbilityTypes.Utility:
                        _utilityAbilityPool.Value.Add(entity);
                        break;
                    case AbilitySystem.AbilityTypes.Terrorize:
                        _terrorizeAbilityPool.Value.Add(entity);
                        break;
                }

                
                if (ability.InputActionReference != null)
                {
                    ref var inputReferenceComp = ref _inputReferencePool.Value.Add(entity);
                    inputReferenceComp.InputActionReference = ability.InputActionReference;
                    if (initEvent.IsReplace || initEvent.initFromCollection)
                    {
                        inputReferenceComp.InputActionReference = initEvent.NewAbilityInputReference;
                    }
                }
                if(ability.PreRequisite != null)
                {
                    ref var preRequisiteComp = ref _preRequisitePool.Value.Add(entity);
                    preRequisiteComp.PreRequisite = ability.PreRequisite.name;
                }

                // if(abilityComp.Ability.SourceAbility.BasicBlocks is not null)
                // {
                    foreach(var block in abilityComp.Ability.SourceAbility.BasicBlocks)
                    {
                        foreach(var comp in block.BasicComponents)
                        {
                            comp.Init(entity, _world.Value);
                        }
                    }
                //}   
                if(_chargePool.Value.Has(entity))
                {
                    _releasedPool.Value.Add(entity);
                    ref var chargeIndicatorComp = ref _chargeIndicatorPool.Value.Add(entity);
                    chargeIndicatorComp.Init();
                    // foreach(var block in abilityComp.Ability.SourceAbility.TimeLineBlocks)
                    // {
                    //     foreach(IZoneReference comp in block.FXComponents)
                    //     {
                    //         comp.GetReference();
                    //     }
                    // }
                    //todo add REFERENCE
                }
                else
                {
                    if(ability.TimerStartsOnPress)
                    {
                        _pressedPool.Value.Add(entity);
                    }
                    else
                    {
                        _releasedPool.Value.Add(entity);
                    }
                }
                if(abilityComp.Ability.IsPlayerAbility) _playerAbilityPool.Value.Add(entity);
            }
        }
    }
}