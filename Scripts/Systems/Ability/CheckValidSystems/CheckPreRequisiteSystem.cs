using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckPreRequisiteSystem : MainEcsSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<PreRequisiteComponent, AbilityComponent, CheckAbilityToUse>, Exc<DeleteCheckAbilityToUseEvent>> _filter = default;
        readonly EcsPoolInject<PreRequisiteComponent> _preRequisitePool = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<DeleteCheckAbilityToUseEvent> _deleteCheckPool = default;
        readonly EcsPoolInject<AbilityUnitComponent> _unitMBPool = default;
        readonly EcsPoolInject<WaitClick> _waitClickPool = default;
        readonly EcsPoolInject<FindPreRequisiteAbilityComponent> _findPool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckPreRequisiteSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    if(!_waitClickPool.Value.Has(ownerEntity) || _findPool.Value.Has(ownerEntity)) 
                    {
                        _deleteCheckPool.Value.Add(entity);
                        continue;
                    }
                    ref var preRequisiteComp = ref _preRequisitePool.Value.Get(entity);
                    ref var unitMBComp = ref _unitMBPool.Value.Get(ownerEntity);
                    if(preRequisiteComp.PreRequisite == unitMBComp.AbilityUnitMB.CurrentAbility)
                    {
                        _findPool.Value.Add(ownerEntity);
                    }
                    else
                    {
                        _deleteCheckPool.Value.Add(entity);
                    }
                }
                else
                {
                    _deleteCheckPool.Value.Add(entity);
                }
                
            }
        }

    }
}