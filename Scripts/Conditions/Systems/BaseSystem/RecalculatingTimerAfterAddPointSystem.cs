using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RecalculatingTimerAfterAddPointSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<AddPointConditionEvent, TimerForResolveComponent,PointsConditionComponent>,Exc<ConditionOverlayToUnitEvent>> _filter;
        readonly EcsPoolInject<TimerForResolveComponent> _timerForResolvePool;
        readonly EcsPoolInject<PointsConditionComponent> _pointConditionPool;
        readonly EcsPoolInject<AddPointConditionEvent> _addPointConditionPool;

        public override MainEcsSystem Clone()
        {
            return new RecalculatingTimerAfterAddPointSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var timerForResolveComp = ref _timerForResolvePool.Value.Get(entity);
                ref var pointConditionComp = ref _pointConditionPool.Value.Get(entity);
                ref var addPointConditionComp = ref _addPointConditionPool.Value.Get(entity);
                var oldCountPoint = pointConditionComp.CurrentPoints - addPointConditionComp.CountPoint;
                if (oldCountPoint <= 0) continue;
                var oldTimeToResolve = timerForResolveComp.TimeToResolve.Evaluate(oldCountPoint);
                if (oldTimeToResolve <= 0) continue;
                var restOfTime = timerForResolveComp.TimerToResolve / oldTimeToResolve;
                timerForResolveComp.TimerToResolve = restOfTime * timerForResolveComp.TimeToResolve.Evaluate(pointConditionComp.CurrentPoints);
            }
        }
    }
}