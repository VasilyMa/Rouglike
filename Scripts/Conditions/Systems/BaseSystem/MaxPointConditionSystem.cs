using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class MaxPointConditionSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<MaxPointConditionEvent>, Exc<MaxPointConditionComponent>> _filter;
        readonly EcsPoolInject<MaxPointConditionComponent> _maxPointConditionPool;

        public override MainEcsSystem Clone()
        {
            return new MaxPointConditionSystem();
        }

        public override  void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _maxPointConditionPool.Value.Add(entity);
            }
        }
    }
}