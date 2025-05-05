using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CheckWithOutPreRequisiteSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<AbilityComponent, CheckAbilityToUse>, Exc<PreRequisiteComponent, DeleteCheckAbilityToUseEvent>> _filter = default;
        readonly EcsPoolInject<OwnerComponent> _ownerPool = default;
        readonly EcsPoolInject<FindPreRequisiteAbilityComponent> _findPool = default;
        readonly EcsPoolInject<DeleteCheckAbilityToUseEvent> _deleteCheckPool = default;

        public override MainEcsSystem Clone()
        {
            return new CheckWithOutPreRequisiteSystem();
        }

        public override void Run (IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var ownerComp = ref _ownerPool.Value.Get(entity);
                if(ownerComp.OwnerEntity.Unpack(_world.Value, out int ownerEntity))
                {
                    if(_findPool.Value.Has(ownerEntity))
                    {
                        _deleteCheckPool.Value.Add(entity);
                    }
                    else
                    {
                        _findPool.Value.Add(ownerEntity);
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