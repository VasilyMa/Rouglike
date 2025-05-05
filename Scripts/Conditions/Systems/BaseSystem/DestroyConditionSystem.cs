using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DestroyConditionSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<DestroyConditionEvent,ConditionCompnent>> _filter;
        readonly EcsPoolInject<ConditionCompnent> _conditionPool;
        readonly EcsPoolInject<ConditionContainerComponent> _conditionContainerPool;
        readonly EcsWorldInject _world;

        public override MainEcsSystem Clone()
        {
            return new DestroyConditionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var conditionComp = ref _conditionPool.Value.Get(entity);
                if (conditionComp.PackedEntityOwner.Unpack(_world.Value, out int entityOwner))
                {
                    if (_conditionContainerPool.Value.Has(entityOwner))
                    {
                        ref var conditionContainer = ref _conditionContainerPool.Value.Get(entityOwner);
                        if (conditionContainer.Conditions.ContainsKey(conditionComp.Condition)) conditionContainer.Conditions.Remove(conditionComp.Condition);
                    }
                }
                _world.Value.DelEntity(entity);
            }
        }
    }
}