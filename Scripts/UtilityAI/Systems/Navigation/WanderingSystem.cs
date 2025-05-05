using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    sealed class WanderingSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<WanderingTag, TransformComponent, EvaluationHelpersData, UnitBrain>> _filter = default;
        readonly EcsPoolInject<EvaluationHelpersData> _dataPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<UnitBrain> _unitbrainPool = default;
        readonly EcsPoolInject<NavMeshComponent> _navMeshPool = default;
        readonly EcsPoolInject<WanderingTag> _wanderingTagPool = default;
        readonly EcsWorldInject _world = default;

        private float _wanderRadius = 10f;
        private float _wanderDelay = 7f;
        private float _stoppingDistance = 1f;

        public override MainEcsSystem Clone()
        {
            return new WanderingSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {

                ref var wandering = ref _wanderingTagPool.Value.Get(entity);
                ref var transformComponent = ref _transformPool.Value.Get(entity);
                ref var data = ref _dataPool.Value.Get(entity);
                ref var unitBrain = ref _unitbrainPool.Value.Get(entity);
                ref var navMeshComp = ref _navMeshPool.Value.Get(entity);

                if (unitBrain.CurrentState != AIState.Wandering)
                {
                    _wanderingTagPool.Value.Del(entity);
                    continue;
                }
                Wandering(ref wandering, ref transformComponent, ref data, ref navMeshComp, ref unitBrain);
            }
        }

        private void Wandering(
            ref WanderingTag wandering,
            ref TransformComponent aiAgentTransform,
            ref EvaluationHelpersData data,
            ref NavMeshComponent navMesh,
            ref UnitBrain unitBrain)
        {
            ref var navMeshAgent = ref navMesh.NavMeshAgent;

            data.WanderingDelay -= Time.deltaTime;

            if (data.WanderingDelay <= 0)
            {
                data.WanderingPos = GetRandomPoint(aiAgentTransform.Transform.position, _wanderRadius);

                data.WanderingDelay = Random.Range(2f, _wanderDelay);
            }
            unitBrain.priorityPointToLook = data.WanderingPos;
            unitBrain.priorityPointToMove = data.WanderingPos;
        }

        private Vector3 GetRandomPoint(Vector3 origin, float radius)
        {
            const int maxAttempts = 30;
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * radius;
                randomDirection += origin;
                randomDirection.y = origin.y; 

                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (NavMesh.CalculatePath(origin, hit.position, NavMesh.AllAreas, path) &&
                        path.status == NavMeshPathStatus.PathComplete)
                    {
                        return hit.position;
                    }
                }
            }
            return origin;
        }

    }
}
