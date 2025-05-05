using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client
{
    sealed class StopAINavigationSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<StopNavigationRequest>> _filter = default;
        readonly private EcsPoolInject<NavigatingTag> _navigatingPool = default;
        readonly private EcsPoolInject<StopNavigationRequest> _stopNavRequestPool = default;
        readonly private EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly private EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new StopAINavigationSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int requestEntity in _filter.Value)
            {
                ref var request = ref _stopNavRequestPool.Value.Get(requestEntity);
                if (request.packedEntity.Unpack(_world.Value, out int unitEntity))
                {
                    if (_navMeshPool.Value.Has(unitEntity))
                    {
                        ref var navMeshComp = ref _navMeshPool.Value.Get(unitEntity);
                        if (navMeshComp.NavMeshAgent.isActiveAndEnabled)
                        {
                            navMeshComp.NavMeshAgent.ResetPath();
                            //navMeshComp.NavMeshAgent.enabled = false; // very bad decision, temporary;
                            //navMeshComp.NavMeshAgent.isStopped = true;
                            //navMeshComp.NavMeshAgent.SetDestination(navMeshComp.NavMeshAgent.transform.position); // bad decision, temporary
                        }
                        _navigatingPool.Value.Del(unitEntity);
                    }
                }
            }
        }
    }
}