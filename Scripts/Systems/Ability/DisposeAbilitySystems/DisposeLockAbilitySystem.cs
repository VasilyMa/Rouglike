using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DisposeLockAbilitySystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DisposeAbilityEvent, TimerAbilityComponent>, Exc<InstantComponent>> _filter = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<DelLockInActionEvent> _delLockPool = default;
        readonly EcsPoolInject<RequestActiveControlEvent> _requestActiveControlPool = default;

        public override MainEcsSystem Clone()
        {
            return new DisposeLockAbilitySystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    if(!_delLockPool.Value.Has(ownerEntity)) _delLockPool.Value.Add(ownerEntity);
                    ref var requestPool = ref _requestActiveControlPool.Value.Add(_world.Value.NewEntity());
                    requestPool.TargetEntity = ownerComp.OwnerEntity;
                }
            }
        }
    }
}