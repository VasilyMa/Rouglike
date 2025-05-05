using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class MoveUnitSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<MoveComponent>, Exc<DeadComponent,PlayerComponent>> _filter = default;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly EcsPoolInject<MoveComponent> _movePool = default;
        public void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var moveComp = ref _movePool.Value.Get(entity);
                ref var NavMeshComp = ref _navMeshPool.Value.Get(entity);

                if(!NavMeshComp.NavMeshAgent.enabled) NavMeshComp.NavMeshAgent.enabled = true;
                if(NavMeshComp.NavMeshAgent.isStopped) NavMeshComp.NavMeshAgent.isStopped = false;
                NavMeshComp.NavMeshAgent.SetDestination(moveComp.TargetPosition);
            }
        }
    }
}