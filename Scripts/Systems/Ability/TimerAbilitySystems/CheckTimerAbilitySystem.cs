using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckTimerAbilitySystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<TimerAbilityComponent>, Exc<DisposeAbilityEvent>> _filter = default;
        readonly EcsPoolInject<TimerAbilityComponent> _timerPool = default;
        readonly EcsPoolInject<ResolveAbilityAfterTimerEvent> _resolvePool = default;
        readonly EcsPoolInject<DisposeAbilityEvent> _disPoseAbilityPool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<DestroyAbilityEvent> _destroyAbilityPool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckTimerAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var timerComp = ref _timerPool.Value.Get(entity);
                if(timerComp.BlocksList.Count > 0)
                {
                    if(timerComp.BlocksList[0].Timer <= timerComp.Timer) _resolvePool.Value.Add(entity);
                }
                else
                {
                    _disPoseAbilityPool.Value.Add(entity);
                    ref var ownerComp = ref _ownerPool.Value.Get(entity);
                    if(!ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                    {
                        ref var destroyAbilityEvt = ref _destroyAbilityPool.Value.Add(_world.Value.NewEntity());
                        destroyAbilityEvt.PackedEntity = _world.Value.PackEntity(entity);
                    }

                }
            }
        }
    }
}