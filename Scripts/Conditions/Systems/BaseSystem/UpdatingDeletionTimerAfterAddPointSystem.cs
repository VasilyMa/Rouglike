using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class UpdatingDeletionTimerAfterAddPointSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<AddPointConditionEvent, RemovePointConditionComponent,PointsConditionComponent>> _filter;
        readonly EcsPoolInject<RemovePointConditionComponent> _removePointConditionPool;
        readonly EcsPoolInject<PointsConditionComponent> _pointsConditionPool;

        public override MainEcsSystem Clone()
        {
            return new UpdatingDeletionTimerAfterAddPointSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var removePointComp =  ref _removePointConditionPool.Value.Get(entity);
                ref var pointsConditionComp = ref _pointsConditionPool.Value.Get(entity);
                removePointComp.TimerToRemove = removePointComp.TimeToRemove.Evaluate(pointsConditionComp.CurrentPoints);
            }
        }
    }
}