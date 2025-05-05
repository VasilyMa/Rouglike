using AbilitySystem;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    sealed class RequestAbilityChildSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<RequestAbilityChildEvent>> _filter;
        readonly EcsPoolInject<RequestAbilityChildEvent> _requestAbilityChildrenPool;
        readonly EcsPoolInject<ChildUnitsComponent> _childUnitsPool;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityComponentPool;
        readonly EcsPoolInject<DisposeAbilityEvent> _disposeEventPool;
        readonly EcsPoolInject<NonWaitClickable> _nonWaitPool;
        readonly EcsPoolInject<AbilityComponent> _abilityPool;
        readonly EcsPoolInject<CoerciveAbilityComponent> _coerciveAbilityPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new RequestAbilityChildSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var requestInvokeComp = ref _requestAbilityChildrenPool.Value.Get(entity);
                if (!requestInvokeComp.CallingEntity.Unpack(_world.Value, out int callingEntity)) continue;
                if (!_childUnitsPool.Value.Has(callingEntity)) continue;
                ref var childUnitsComp = ref _childUnitsPool.Value.Get(callingEntity);/// ETO KAKAI TO PIZDA NO I STARALSI
                foreach(var childPackedEntity in childUnitsComp.childUnits)
                {
                    if (!childPackedEntity.Unpack(_world.Value, out int childEntity)) continue;
                    ref var abilityComponent = ref _abilityComponentPool.Value.Get(childEntity);
                    foreach (var packedEntityList in abilityComponent.AbilityUnitMB.AllAbilities.Values)
                    {
                        foreach (var abilityPAckedEntity in packedEntityList)
                        {
                            if (!abilityPAckedEntity.Unpack(_world.Value, out int unpackedAbilityEntity)) continue;
                            ref var abilityComp = ref _abilityPool.Value.Get(unpackedAbilityEntity);
                            if (abilityComp.Ability.SourceAbility.Name != requestInvokeComp.InvokeAbilityName)
                            {
                                if (!_disposeEventPool.Value.Has(unpackedAbilityEntity)) _disposeEventPool.Value.Add(unpackedAbilityEntity);
                                if (!_nonWaitPool.Value.Has(unpackedAbilityEntity)) _nonWaitPool.Value.Add(unpackedAbilityEntity);
                                continue;
                            }
                            if (!_coerciveAbilityPool.Value.Has(childEntity)) _coerciveAbilityPool.Value.Add(childEntity);
                            ref var coerciveAbilityComp = ref _coerciveAbilityPool.Value.Get(childEntity);
                            coerciveAbilityComp.EntityAbility = unpackedAbilityEntity;
                        }
                    }

                }
            }
        }
    }
}
