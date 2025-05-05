using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;
using AbilitySystem;
using UniRx;
using System.Collections.Generic;

namespace Client 
{
    sealed class InitObserver : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<PlayerComponent, InitUnitEvent>> _filter = default;
        readonly EcsPoolInject<PlayerObserverComponent> _observerPool = default;
        readonly EcsPoolInject<AbilityObserverComponent> _abilityObserverPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        private List<CurrentAbilitiesData> _currentAbilitiesDataTemporaryCopy = new List<CurrentAbilitiesData>();

        public override MainEcsSystem Clone()
        {
            return new InitObserver();
        }
        private string GetDataFromTemporaryListWithDeletion(string ID)
        {
            var dataIndex = _currentAbilitiesDataTemporaryCopy.FindIndex(x => x.KEY_ID == ID);
            string inputActionReference = _currentAbilitiesDataTemporaryCopy[dataIndex].ReferenceInput.action.name;
            _currentAbilitiesDataTemporaryCopy.RemoveAt(dataIndex);
            return inputActionReference;
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                _currentAbilitiesDataTemporaryCopy.Clear();
                //Init player observer
                ref var playerObserver = ref _observerPool.Value.Add(entity);
                playerObserver.HealthValue = new ReactiveProperty<HealthValue>();
                var observer = new PlayerObserver();
                observer.SetPlayerProperty(playerObserver.HealthValue);
                ObserverEntity.Instance.AddPlayer(observer);

                //Init ability observers
                ref var abilityUnitComp = ref _abilityUnitPool.Value.Get(entity);

                int index = 0;
                AbilityCollectionData data = PlayerEntity.Instance.AbilityCollectionData;
                bool abilitiesAlreadyExist = false;
                if (data.CurrentAbilitiesData.Count > 0)
                {
                    _currentAbilitiesDataTemporaryCopy = new List<CurrentAbilitiesData>(data.CurrentAbilitiesData);
                    abilitiesAlreadyExist = true;
                }
                
                foreach (var ability in abilityUnitComp.AbilityUnitMB.AllAbilities)
                {
                    foreach (var ecsPackedEntity in ability.Value)
                    {
                        if (ecsPackedEntity.Unpack(_world.Value, out int abilityEntity))
                        {
                            ref var abilityComp = ref _abilityPool.Value.Get(abilityEntity);

                            if (abilityComp.Ability.SourceAbility.PreRequisite != null) continue;

                            ref var abilityObserver = ref _abilityObserverPool.Value.Add(abilityEntity);

                            string value = string.Empty;

                            if (abilityComp.Ability.SourceAbility.IconAbility != null)
                            {
                                value = abilityComp.Ability.SourceAbility.IconAbility.name;
                            }

                            abilityObserver.CooldownValue = new ReactiveProperty<CooldownValue>();
                            abilityObserver.ChargeValue = new ReactiveProperty<ChargeValue>();
                            string inputActionReferenceName = abilitiesAlreadyExist ? GetDataFromTemporaryListWithDeletion(abilityComp.Ability.KEY_ID) : abilityComp.Ability.SourceAbility.InputActionReference.action.name;
                            abilityObserver.AbilityObserver = new AbilityObserver(abilityObserver.CooldownValue, abilityObserver.ChargeValue, index, value, inputActionReferenceName);
                            ObserverEntity.Instance.AddAbility(abilityObserver.AbilityObserver);
                            //ObserverEntity.Instance.Abilities.Add());
                        }
                    }

                    index++;
                }
            }
        }
    }
}