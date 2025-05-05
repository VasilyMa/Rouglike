using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RemovePointSystem : MainEcsSystem {
        readonly EcsFilterInject<Inc<RemovePointConditionComponent,PointsConditionComponent>,Exc<DestroyConditionEvent>> _filter;
        readonly EcsPoolInject<RemovePointConditionComponent> _removePointPool;
        readonly EcsPoolInject<DestroyConditionEvent> _destroyConditionPool;
        readonly EcsPoolInject<PointsConditionComponent> _pointConditionPool;
        readonly EcsPoolInject<MaxPointConditionComponent> _mapPointConditionPool;

        public override MainEcsSystem Clone()
        {
            return new RemovePointSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var removePointComp = ref _removePointPool.Value.Get(entity);
                removePointComp.TimerToRemove -= Time.deltaTime;
                if (removePointComp.TimerToRemove > 0) continue;
                ref var pointConditionComp = ref _pointConditionPool.Value.Get(entity);
                removePointComp.TimerToRemove = removePointComp.TimeToRemove.Evaluate(pointConditionComp.CurrentPoints); 
                pointConditionComp.CurrentPoints -= removePointComp.CountPoint;
                if (pointConditionComp.MaxPoints != pointConditionComp.CurrentPoints) _mapPointConditionPool.Value.Del(entity);
                if (pointConditionComp.CurrentPoints <= 0 || removePointComp.allPointRemove) _destroyConditionPool.Value.Add(entity);
            }
        }
    }
}