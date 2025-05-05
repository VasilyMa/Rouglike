using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AddPointConditionSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AddPointConditionEvent, PointsConditionComponent>,Exc<MaxPointConditionEvent>> _filter;
        readonly EcsPoolInject<AddPointConditionEvent> _addPointConditionPool;
        readonly EcsPoolInject<PointsConditionComponent> _pointConditionPool;
        readonly EcsPoolInject<MaxPointConditionEvent> _maxPointPool;

        public override MainEcsSystem Clone()
        {
            return new AddPointConditionSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var addPointEvent = ref _addPointConditionPool.Value.Get(entity);
                ref var pointConditionComp = ref _pointConditionPool.Value.Get(entity);
                pointConditionComp.CurrentPoints += addPointEvent.CountPoint;
                if (pointConditionComp.CurrentPoints < pointConditionComp.MaxPoints) continue;
                pointConditionComp.CurrentPoints = pointConditionComp.MaxPoints;
                _maxPointPool.Value.Add(entity);

            }
        }
    }
}