using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class ConditionOverlayToUnitSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ConditionOverlayToUnitEvent, RequestConditionOverlayEvent>> _filter;
        readonly EcsPoolInject<RequestConditionOverlayEvent> _requestConditionOverlayPool;
        readonly EcsPoolInject<ConditionOverlayToUnitEvent> _conditionOverlayToUnitPool;
        readonly EcsPoolInject<ConditionCompnent> _conditionPool;
        readonly EcsPoolInject<PointsConditionComponent> _pointConditionPool;
        readonly EcsPoolInject<AddPointConditionEvent> _addPointCondtitionPool;
        readonly EcsPoolInject<ConditionContainerComponent> _conditionContainerPool;
        readonly EcsPoolInject<ViewConditionComponent> _viewConditionPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new ConditionOverlayToUnitSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var requestConditionsOverlayComp = ref _requestConditionOverlayPool.Value.Get(entity);
                ref var conditionOverlayToUnit = ref _conditionOverlayToUnitPool.Value.Get(entity);

                if (!requestConditionsOverlayComp.OwnerEntity.Unpack(_world.Value, out int entityOwner)) continue;
                if (!_conditionContainerPool.Value.Has(entityOwner)) continue;
                ref var conditionContainerComp = ref _conditionContainerPool.Value.Get(entityOwner);
                conditionContainerComp.Conditions.Add(requestConditionsOverlayComp.Condition, _world.Value.PackEntity(entity));

                ref var conditionComp = ref _conditionPool.Value.Add(entity);
                conditionComp.PackedEntityOwner = requestConditionsOverlayComp.OwnerEntity;
                conditionComp.Condition = requestConditionsOverlayComp.Condition;

                ref var pointConditionComp = ref _pointConditionPool.Value.Add(entity);
                pointConditionComp.CurrentPoints = 0;
                pointConditionComp.MaxPoints = conditionOverlayToUnit.ConditionSettings.MaxPoint;
                _viewConditionPool.Value.Add(entity);

                foreach (var conditionComponent in conditionOverlayToUnit.ConditionSettings.Components)
                {
                    conditionComponent.Invoke(entity, _world.Value);
                }
                _addPointCondtitionPool.Value.Add(entity).CountPoint = requestConditionsOverlayComp.StartCountPoint;
            }
        }
    }
}