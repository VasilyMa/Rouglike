using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using Statement;
using UniRx;
using UnityEngine.InputSystem;

namespace Client {
    sealed class AddAbilityToOwnerList : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<InitAbilityEvent, InputReferenceComponent, OwnerComponent>> _filter = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _unitMBPool = default;
        readonly EcsPoolInject<InputReferenceComponent> _inputReferencePool = default;
        readonly EcsPoolInject<DestroyAbilityEvent> _destroyAbilityPool = default;
        readonly EcsPoolInject<InitAbilityEvent> _initAbilityPool = default;
        readonly EcsPoolInject<AbilityObserverComponent> _observerPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<PlayerComponent> _playerPool = default;
        readonly EcsPoolInject<InputComponent> _inputPool = default;

        public override MainEcsSystem Clone()
        {
            return new AddAbilityToOwnerList();
        }


        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                //добавляем абилку в лист абилок у юнита(хранится в UnitMB пока)
                ref var ownerComp = ref _ownerPool.Value.Get(entity);

                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int unpackedOwnerEntity))
                {
                    ref var initAbilityComp = ref _initAbilityPool.Value.Get(entity);
                    ref var inputReferenceComp = ref _inputReferencePool.Value.Get(entity);
                    ref var unitMBComp = ref _unitMBPool.Value.Get(unpackedOwnerEntity);
                    //пытаемся добавить в лист, если нет ключа добавим, если есть перепишем значение на новую энтити
                    EcsPackedEntity packedAbilityEntity = _world.Value.PackEntity(entity);

                    InputActionReference inputReference = inputReferenceComp.InputActionReference;
                    string inputReferenceKey = inputReferenceComp.InputActionReference.action.name;
                    string ability_key_id = initAbilityComp.AbilityBase.KEY_ID;
                    ref var inputComp = ref _inputPool.Value.Get(State.Instance.GetEntity("InputEntity"));
                    if (initAbilityComp.IsReplace)
                    {
                        //AbilityObserver abilityObserver = null;
                        ReactiveProperty<CooldownValue> cooldownValue = null;
                        ReactiveProperty<ChargeValue> chargeValue = null;
                        int index = 0;
                        if (unitMBComp.AbilityUnitMB.AllAbilities.ContainsKey(inputReferenceKey))
                        {
                            foreach (var packedEntityAbility in unitMBComp.AbilityUnitMB.AllAbilities[inputReferenceKey])
                            {
                                if (packedEntityAbility.Unpack(State.Instance.EcsRunHandler.World, out var abilityEntityOld))
                                {
                                    if (_observerPool.Value.Has(abilityEntityOld))
                                    {
                                        ref var observerComp = ref _observerPool.Value.Get(abilityEntityOld);
                                        
                                        //abilityObserver = observerComp.AbilityObserver;
                                        index = observerComp.AbilityObserver.AbilityInfo.index;
                                        cooldownValue = observerComp.CooldownValue;
                                        chargeValue = observerComp.ChargeValue;
                                        ObserverEntity.Instance.RemoveAbility(observerComp.AbilityObserver);
                                        _observerPool.Value.Del(abilityEntityOld);
                                    }
                                }
                            }

                            unitMBComp.AbilityUnitMB.AllAbilities[inputReferenceKey].Clear();
                        }

                        List<EcsPackedEntity> list = new List<EcsPackedEntity>();
                        unitMBComp.AbilityUnitMB.AllAbilities[inputReferenceKey].Add(packedAbilityEntity);

                        if (packedAbilityEntity.Unpack(State.Instance.EcsRunHandler.World, out var abilityNewEntity))
                        {
                            ref var abilityObserverComp = ref _observerPool.Value.Add(abilityNewEntity);
                            ref var abilityComp = ref _abilityPool.Value.Get(abilityNewEntity);
                            string value = string.Empty;

                            if (abilityComp.Ability.SourceAbility.IconAbility != null)
                            {
                                value = abilityComp.Ability.SourceAbility.IconAbility.name;
                            }
                            AbilityObserver abilityObserver = new AbilityObserver(cooldownValue, chargeValue, index, abilityComp.Ability.SourceAbility.IconAbility.name, inputReferenceKey);
                            abilityObserverComp.CooldownValue = cooldownValue;
                            abilityObserverComp.ChargeValue = chargeValue;
                            abilityObserverComp.AbilityObserver = abilityObserver;
                            
                            abilityObserver.SetNewIcon(value);
                            abilityObserver.SetNewActionName(inputReferenceKey);
                            ObserverEntity.Instance.AddAbility(abilityObserver);
                        }
                    }
                    else
                    {
                        if (!unitMBComp.AbilityUnitMB.AllAbilities.ContainsKey(inputReferenceKey))
                        {
                            List<EcsPackedEntity> list = new List<EcsPackedEntity>();
                            unitMBComp.AbilityUnitMB.AllAbilities.Add(inputReferenceKey, list);
                        }
                        unitMBComp.AbilityUnitMB.AllAbilities[inputReferenceKey].Add(packedAbilityEntity);
                    }

                    if (_playerPool.Value.Has(unpackedOwnerEntity))
                    {
                        PlayerEntity.Instance.AbilityCollectionData.AddTemporaryAbility
                            (new CurrentAbilitiesData(ability_key_id, 1, inputReference), inputReferenceKey == inputComp.InputAction.ActionMap.Attack.name);
                    }
                }
            }
        }
    }
}