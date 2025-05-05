using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class UpdateThreatsContextSystem : MainEcsSystem
    {
        readonly private EcsFilterInject<Inc<UnitBrain, ThreatsContext, TransformComponent>> _unitFilter = default;
        readonly private EcsPoolInject<ThreatsContext> _threatsContextPool = default;
        readonly private EcsFilterInject<Inc<MissileComponent, TransformComponent>> _missileFilter = default;
        readonly private EcsFilterInject<Inc<PlayerComponent, AttackRequest>> _attackingUnitsFilter = default;
        readonly private EcsPoolInject<EvaluationHelpersData> _helperPool = default;
        readonly private EcsPoolInject<TransformComponent> _transformPool = default;
        readonly private EcsPoolInject<MissileComponent> _missilePool = default;
        readonly private float _meleeThreatRadius = 2f;
        readonly private float _meleeAttackRadius = 2f;

        public override MainEcsSystem Clone()
        {
            return new UpdateThreatsContextSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (int unitEntity in _unitFilter.Value)
            {
                ref var threatsContext = ref _threatsContextPool.Value.Get(unitEntity);
                ref var helperData = ref _helperPool.Value.Get(unitEntity);
                ref var transformComp = ref _transformPool.Value.Get(unitEntity);
                bool isUnderThreat = false;
                bool isThreatMelee = false;

                foreach (int attackingPlayerEntity in _attackingUnitsFilter.Value)
                {
                    ref var attackingPlayerTransform = ref _transformPool.Value.Get(attackingPlayerEntity);
                    if (IsAttackThreatens(attackingPlayerTransform.Transform, transformComp.Transform))
                    {
                        isUnderThreat = true;
                        isThreatMelee = true;
                        threatsContext.incomingDirection = (transformComp.Transform.position - attackingPlayerTransform.Transform.position).normalized;
                    }
                }

                if (!isUnderThreat)
                {
                    foreach (int missileEntity in _missileFilter.Value)
                    {
                        ref var missileComp = ref _missilePool.Value.Get(missileEntity);
                        if (missileComp.LayerNameTarget != "Enemy") continue;

                        

                        ref var missileTransform = ref _transformPool.Value.Get(missileEntity);
                        Vector3 projectilePosition = missileTransform.Transform.position;
                        Vector3 projectileDirection = missileTransform.Transform.forward;

                        Vector3 closestPoint = GetClosestPointOnLine(projectilePosition, projectileDirection, transformComp.Transform.position);
                        float distanceToClosestPoint = Vector3.Distance(closestPoint, transformComp.Transform.position);

                        if (distanceToClosestPoint < 2.5f)
                        {
                            helperData.ReactionDelay -=  Time.deltaTime;

                            if (helperData.ReactionDelay > 0) continue;

                            helperData.ReactionDelay = helperData.ReactionToAction;

                            threatsContext.incomingDirection = EvasiveManeuver(closestPoint, ref transformComp.Transform, ref helperData);
                            threatsContext.incomingDirection.y = 0;
                            isUnderThreat = true;
                        }
                    }
                }

                threatsContext.isUnderThreat = isUnderThreat;
                threatsContext.isThreatMelee = isThreatMelee;
            }
        }

        private bool IsAttackThreatens(Transform attackingTransform, Transform defendingTransform)
        {
            Vector3 attackEndPoint = attackingTransform.position + attackingTransform.forward * _meleeAttackRadius;
            return Vector3.Distance(defendingTransform.position, attackEndPoint) < _meleeThreatRadius;
        }

        private Vector3 GetClosestPointOnLine(Vector3 lineStart, Vector3 lineDirection, Vector3 point)
        {
            Vector3 toPoint = point - lineStart;
            float dotProduct = Vector3.Dot(toPoint, lineDirection);
            Vector3 closestPoint = lineStart + dotProduct * lineDirection;
            return closestPoint;
        }

        private Vector3 EvasiveManeuver(Vector3 closestPoint, ref Transform transform, ref EvaluationHelpersData data)
        {
            Vector3 evasiveDirection = (transform.position - closestPoint).normalized;
            return evasiveDirection;
        }
    }
}
