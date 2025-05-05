using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class DisableNavMeshSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<NavMeshComponent, MomentDeadEvent>> _filter;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool;

        public override MainEcsSystem Clone()
        {
            return new DisableNavMeshSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var navMeshComp = ref _navMeshPool.Value.Get(entity);
                if (navMeshComp.NavMeshAgent.isActiveAndEnabled) navMeshComp.NavMeshAgent.isStopped = true;
                navMeshComp.NavMeshAgent.ResetPath();
                navMeshComp.NavMeshAgent.enabled = false;
            }
        }
    }
}