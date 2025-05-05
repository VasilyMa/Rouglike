using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DisposeHitIntervalSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<DisposeHitIntervalEvent>> _filter;
        readonly EcsPoolInject<ApprovedDashAfterHitComponent> _approvedDashPool;
        readonly EcsPoolInject<TimerBeforeApprovedDashComponent> _timerBeforeApprovedDashPool;
        readonly EcsPoolInject<HardHitComponent> _hardHitPool;

        public override MainEcsSystem Clone()
        {
            return new DisposeHitIntervalSystem();
        }
        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                if (_approvedDashPool.Value.Has(entity)) _approvedDashPool.Value.Del(entity);
                if (_timerBeforeApprovedDashPool.Value.Has(entity)) _timerBeforeApprovedDashPool.Value.Del(entity);
                if (_hardHitPool.Value.Has(entity)) _hardHitPool.Value.Del(entity);
            }
        }
    }
}