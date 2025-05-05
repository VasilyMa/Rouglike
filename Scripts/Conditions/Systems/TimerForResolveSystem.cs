using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class TimerForResolveSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<TimerForResolveComponent, PointsConditionComponent>> _filter;
        readonly EcsPoolInject<ResolveConditionEvent> _resolveConditionPool;
        readonly EcsPoolInject<TimerForResolveComponent> _timeForResolvePool;
        readonly EcsPoolInject<PointsConditionComponent> _pointsConditionPool;

        public override MainEcsSystem Clone()
        {
            return new TimerForResolveSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var timerForResolveComp = ref _timeForResolvePool.Value.Get(entity);
                ref var pointConditionComp = ref _pointsConditionPool.Value.Get(entity);
                timerForResolveComp.TimerToResolve -= Time.deltaTime;
                if (timerForResolveComp.TimerToResolve > 0) continue;
                timerForResolveComp.TimerToResolve = timerForResolveComp.TimeToResolve.Evaluate(pointConditionComp.CurrentPoints);
                if (!_resolveConditionPool.Value.Has(entity)) _resolveConditionPool.Value.Add(entity).resolve = new();
                ref var resolveConditionComp = ref _resolveConditionPool.Value.Get(entity);
                resolveConditionComp.resolve.AddRange(timerForResolveComp.Resolve);
            }
        }
    }
}