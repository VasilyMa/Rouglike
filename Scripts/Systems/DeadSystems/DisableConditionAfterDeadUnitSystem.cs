using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DisableConditionAfterDeadUnitSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MomentDeadEvent, ConditionContainerComponent>> _filter;
        readonly EcsPoolInject<ConditionContainerComponent> _conditionContainerPool;
        readonly EcsPoolInject<DestroyConditionEvent> _destroyConditionPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new DisableConditionAfterDeadUnitSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var conditionContainerComp = ref _conditionContainerPool.Value.Get(entity);
                foreach(var condition in conditionContainerComp.Conditions.Values)
                {
                    if (!condition.Unpack(_world.Value, out int conditionEntity)) continue;
                    if (!_destroyConditionPool.Value.Has(conditionEntity)) _destroyConditionPool.Value.Add(conditionEntity);
                }
            }
        }
    }
}