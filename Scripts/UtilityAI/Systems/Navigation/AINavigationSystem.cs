using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Client {
    sealed class AINavigationSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, NavigatingTag, TransformComponent>,Exc<LockMoveComponent,SpawnAbilityEvent>> _filter = default;
        readonly private EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly private EcsPoolInject<UnitBrain> _brainPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new AINavigationSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (int unitEntity in _filter.Value)
            {
                ref var navMeshComp = ref _navMeshPool.Value.Get(unitEntity);
                ref var brainComp = ref _brainPool.Value.Get(unitEntity);
                if (!NavMesh.SamplePosition(brainComp.priorityPointToMove, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    
                    ref var transformComp = ref _transformPool.Value.Get(unitEntity);
                    brainComp.priorityPointToMove = transformComp.Transform.position;
                }
                else
                {
                    brainComp.priorityPointToMove = hit.position;
                }
                navMeshComp.NavMeshAgent.SetDestination(brainComp.priorityPointToMove);

            }
        }
    }
}