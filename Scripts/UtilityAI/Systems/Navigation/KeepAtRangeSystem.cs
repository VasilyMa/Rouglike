using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client
{
    sealed class KeepAtRangeSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<UnitBrain, KeepingAtRangeTag, TransformComponent, EvaluationHelpersData>> _filter = default;
        readonly EcsPoolInject<EvaluationHelpersData> _dataPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<UnitBrain> _unitBrainPool = default;
        readonly EcsPoolInject<KeepingAtRangeTag> _keepingAtRangeTagPool = default;
        readonly EcsWorldInject _world = default;

        public override MainEcsSystem Clone()
        {
            return new KeepAtRangeSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter.Value)
            {
                ref var unitBrain = ref _unitBrainPool.Value.Get(entity);
                if (unitBrain.CurrentState != AIState.KeepAtRange)
                {
                    _keepingAtRangeTagPool.Value.Del(entity);
                    continue;
                }
                ref var keepingAtRange = ref _keepingAtRangeTagPool.Value.Get(entity);
                ref var transformComponent = ref _transformPool.Value.Get(entity);
                ref var data = ref _dataPool.Value.Get(entity);
                UpdateCircularMovement(ref keepingAtRange, ref unitBrain, ref transformComponent, ref data);
                UpdateDirectionChange(ref keepingAtRange);
            }
        }

        private void UpdateCircularMovement
            (
                ref KeepingAtRangeTag keepingAtRange, ref UnitBrain unitBrain, ref TransformComponent aiAgentTransform, ref EvaluationHelpersData data
            )
        {
            keepingAtRange.distanceToKeep = 1;
            Vector3 currentDistanceVector = aiAgentTransform.Transform.position - keepingAtRange.transformToKeepAtRange.position;
            Vector3 rotatedVector = Quaternion.Euler(0f, Time.deltaTime * keepingAtRange.directionModifier, 0f) * currentDistanceVector;
            Vector3 movementVector = ((keepingAtRange.transformToKeepAtRange.position + rotatedVector) - aiAgentTransform.Transform.position).normalized;
            Vector3 avoidanceVector = currentDistanceVector.normalized;
            float avoidancePart = data.CowardiceScoreByDistance.Evaluate(Vector3.Distance(aiAgentTransform.Transform.position, keepingAtRange.transformToKeepAtRange.position));
            float movementPart = 1f - avoidancePart;
            Vector3 movementAndAvoidance = (movementVector * movementPart + avoidanceVector * avoidancePart).normalized + currentDistanceVector.normalized * keepingAtRange.distanceToKeep; // multiplying for increasing movement contribution in finalized vector
                                                                                                                                                                                            //TODOihor add vectors distribution dependent on distance
            unitBrain.priorityPointToMove = aiAgentTransform.Transform.position + movementAndAvoidance;
            unitBrain.priorityPointToLook = keepingAtRange.transformToKeepAtRange.position;

        }

        private void UpdateDirectionChange(ref KeepingAtRangeTag keepingAtRange)
        {
            keepingAtRange.timeLeftToDirectionSwitch -= Time.deltaTime;
            if (keepingAtRange.timeLeftToDirectionSwitch < 0f)
            {
                keepingAtRange.directionModifier *= -2;
                keepingAtRange.timeLeftToDirectionSwitch = Random.Range(1f, 3f); //TODOihor get it somewhere else
            }
        }
    }
}