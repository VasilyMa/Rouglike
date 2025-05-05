using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class TimerBeforApprovedDashSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<TimerBeforeApprovedDashComponent>,Exc<ApprovedDashAfterHitComponent>> _filter;
        readonly EcsPoolInject<TimerBeforeApprovedDashComponent> _timerBefore;
        readonly EcsPoolInject<ApprovedDashAfterHitComponent> _approvedDashAfterHitPool;
        public override MainEcsSystem Clone()
        {
            return new TimerBeforApprovedDashSystem();
        }
        
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var timerBeforApprovedDash = ref _timerBefore.Value.Get(entity);
                timerBeforApprovedDash.TimerBeforApproved -= Time.deltaTime;
                if (timerBeforApprovedDash.TimerBeforApproved > 0) continue;
                ref var approvedDashComp = ref _approvedDashAfterHitPool.Value.Add(entity);
                approvedDashComp.TimerApprovedDash = timerBeforApprovedDash.TimeApprovedDash;
                _timerBefore.Value.Del(entity);
            }
        }
    }
}