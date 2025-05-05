using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DisposeAllAbilityOnUnitSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DisposeAllAbilityOnUnitEvent>> _filter = default;
        readonly EcsPoolInject<DisposeAllAbilityOnUnitEvent> _disposePool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityComponentPool = default;
        readonly EcsPoolInject<DisposeAbilityEvent> _disposeEventPool = default;
        readonly EcsPoolInject<NonWaitClickable> _nonWaitPool = default;
        public override MainEcsSystem Clone()
        {
            return new DisposeAllAbilityOnUnitSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var disposeComp = ref _disposePool.Value.Get(entity);
                if(disposeComp.OwnerEntity.Unpack(_world.Value, out int targetEntity))
                {
                    ref var abilityComponent = ref _abilityComponentPool.Value.Get(targetEntity);
                    foreach (var packedEntityList in abilityComponent.AbilityUnitMB.AllAbilities.Values)
                    {
                        foreach (var abilityPAckedEntity in packedEntityList)
                        {
                            if (abilityPAckedEntity.Unpack(_world.Value, out int unpackedAbilityEntity))
                            {
                                if (!_disposeEventPool.Value.Has(unpackedAbilityEntity)) _disposeEventPool.Value.Add(unpackedAbilityEntity);
                                if (!_nonWaitPool.Value.Has(unpackedAbilityEntity)) _nonWaitPool.Value.Add(unpackedAbilityEntity);
                            }
                        }
                    }
                }

            }
        }
    }
}