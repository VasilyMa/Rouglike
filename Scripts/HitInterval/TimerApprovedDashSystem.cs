using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class TimerApprovedDashSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<ApprovedDashAfterHitComponent>> _filter;
        readonly EcsPoolInject<ApprovedDashAfterHitComponent> _approvedDashPool;
        public override MainEcsSystem Clone()
        {
            return new TimerApprovedDashSystem();
        }
        public override void Run(IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var approvedDashComp = ref _approvedDashPool.Value.Get(entity);
                approvedDashComp.TimerApprovedDash -= Time.deltaTime;
                if (approvedDashComp.TimerApprovedDash > 0) continue;
                _approvedDashPool.Value.Del(entity);
            }
        }
    }
}