using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class DisposeAbilitySystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DisposeAbilityEvent, TimerAbilityComponent>> _filter = default;
        readonly EcsPoolInject<TimerAbilityComponent> _timerPool = default;
        readonly EcsPoolInject<AbilityComponent> _abilityPool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;

        public override MainEcsSystem Clone()
        {
            return new DisposeAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    ref var abilityComp = ref _abilityPool.Value.Get(entity);

                    foreach(var blocks in abilityComp.Ability.SourceAbility.InputBlocks)
                    {
                        foreach (var comp in blocks.Components)
                        {
                            comp.Dispose(ownerEntity, entity,  _world.Value);
                        }
                    }

                    foreach(var blocks in abilityComp.Ability.SourceAbility.TimeLineBlocks)
                    {
                        foreach (var comp in blocks.FXComponents)
                        {
                            comp.Dispose(ownerEntity, entity,_world.Value);
                        }
                    }
                }
                
                _timerPool.Value.Del(entity);
            }
        }
    }
}