using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class StartAINavigationSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<StartNavigationRequest>,Exc<HardControlComponent>> _requestFilter = default; // exc norm chi net??? podumai dvazhdi brat
        readonly private EcsPoolInject<StartNavigationRequest> _startNavigationRequestPool = default;
        readonly private EcsPoolInject<NavigatingTag> _navigatingTagPool = default;
        readonly private EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly private EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new StartAINavigationSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int requestEntity in _requestFilter.Value)
            {
                ref var request = ref _startNavigationRequestPool.Value.Get(requestEntity);
                if (request.packedEntity.Unpack(_world.Value, out int unitEntity))
                {
                    if (!_navigatingTagPool.Value.Has(unitEntity))
                    {
                        _navigatingTagPool.Value.Add(unitEntity);
                        if (_navMeshPool.Value.Has(unitEntity))
                        {
                            ref var navMeshComp = ref _navMeshPool.Value.Get(unitEntity);
                            if (!navMeshComp.NavMeshAgent.isActiveAndEnabled) navMeshComp.NavMeshAgent.enabled = true;
                            navMeshComp.NavMeshAgent.isStopped = false;
                        }
                    }
                }
            }
        }
    }
}