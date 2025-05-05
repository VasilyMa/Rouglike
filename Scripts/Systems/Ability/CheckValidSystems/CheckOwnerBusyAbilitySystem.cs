using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckOwnerBusyAbilitySystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<OwnerComponent, CheckAbilityToUse>, Exc<InstantComponent, DeleteCheckAbilityToUseEvent>> _filter = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<InActionComponent> _inActionPool = default;
        readonly EcsPoolInject<HardControlComponent> _hardControlPool = default;
        readonly EcsPoolInject<DeleteCheckAbilityToUseEvent> _deleteCheckPool = default;
        readonly EcsPoolInject<DeadComponent> _deadPool = default;
        readonly EcsPoolInject<MomentDeadEvent> _momentDeadPool = default;
        readonly EcsPoolInject<ApprovedInvokeInHitComponent> _approvedInvokeInHitPool;
        readonly EcsPoolInject<ApprovedDashAfterHitComponent> _approvedDashAfterHitPool;

        public override MainEcsSystem Clone()
        {
            return new CheckOwnerBusyAbilitySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    //todo проверочка на занатость игрока, если занят то снять компонент
                    if (_inActionPool.Value.Has(ownerEntity)) _deleteCheckPool.Value.Add(entity);
                    else if (_hardControlPool.Value.Has(ownerEntity))
                    {
                        if (!_approvedInvokeInHitPool.Value.Has(entity)) _deleteCheckPool.Value.Add(entity);
                        else if (!_approvedDashAfterHitPool.Value.Has(ownerEntity)) _deleteCheckPool.Value.Add(entity);
                    }
                    else if (_deadPool.Value.Has(ownerEntity)) _deleteCheckPool.Value.Add(entity);
                    else if (_momentDeadPool.Value.Has(ownerEntity)) _deleteCheckPool.Value.Add(entity);
                }
                else
                {
                    _deleteCheckPool.Value.Add(entity);
                }

            }
        }
    }
}