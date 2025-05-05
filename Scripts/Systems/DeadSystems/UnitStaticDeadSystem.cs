using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UnitStaticDeadSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<StaticUnitComponent, MomentDeadEvent>> _filter;
        readonly EcsPoolInject<DelEntityEvent> _delEvent;
        readonly EcsPoolInject<StaticUnitComponent> _staticUnitPool;
        readonly EcsPoolInject<ChildUnitsComponent> _childUnitsPool;
        readonly EcsWorldInject _world;
        readonly EcsPoolInject<MomentDeadEvent> _momentDeadPool;

        public override MainEcsSystem Clone()
        {
            return new UnitStaticDeadSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _delEvent.Value.Add(entity);
                ref var staticUnitComp = ref _staticUnitPool.Value.Get(entity);
                if (!staticUnitComp.ownerEntity.Unpack(_world.Value, out int ownerEntity)) continue;
                if (!_childUnitsPool.Value.Has(ownerEntity)) continue;
                ref var childComp = ref _childUnitsPool.Value.Get(ownerEntity);
                childComp.childUnits.Remove(_world.Value.PackEntity(entity));
                if (childComp.childUnits.Count != 0) continue;
                _childUnitsPool.Value.Del(ownerEntity);
            }
        }
    }
}