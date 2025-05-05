using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DeleteAbilitiesAfterDeadSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<MomentDeadEvent, AbilityUnitComponent>> _filter = default;
        readonly EcsPoolInject<DestroyAbilityEvent> _destroyAbilityPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _abilityUnitPool = default;

        public override MainEcsSystem Clone()
        {
            return new DeleteAbilitiesAfterDeadSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var abilityComp = ref _abilityUnitPool.Value.Get(entity);
                foreach (var abilityPackedEntityList in abilityComp.AbilityUnitMB.AllAbilities.Values)
                {
                    foreach (var abilityPackedEntity in abilityPackedEntityList)
                    {
                        if(abilityPackedEntity.Unpack(_world.Value, out int abilityEntity))
                        {
                            ref var destroyAbilityEvt = ref _destroyAbilityPool.Value.Add(_world.Value.NewEntity());
                            destroyAbilityEvt.PackedEntity = abilityPackedEntity;
                        }
                    }
                }
            }
        }
    }
}