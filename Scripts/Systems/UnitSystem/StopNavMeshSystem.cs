using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class StopNavMeshSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<UnitComponent>, Exc<DeadComponent, MoveComponent>> _filter = default;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        public void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var viewComp = ref _navMeshPool.Value.Get(entity);
                ref var transfromCopm = ref _transformPool.Value.Get(entity);
                if (viewComp.NavMeshAgent.isOnNavMesh) viewComp.NavMeshAgent.isStopped = true;
                if (viewComp.NavMeshAgent.isActiveAndEnabled) 
                viewComp.NavMeshAgent.SetDestination(transfromCopm.Transform.position);
            }
        }
    }
}