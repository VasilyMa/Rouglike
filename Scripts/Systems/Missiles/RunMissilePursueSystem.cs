using Leopotam.EcsLite;
using static Michsky.UI.Heat.GradientFilter;
using UnityEngine;
using Leopotam.EcsLite.Di;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Client {
    sealed class RunMissilePursueSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MissileComponent,MissilePursueComponent>> _filter = default;
        readonly EcsPoolInject<MissileComponent> _missilePool = default;
        readonly EcsPoolInject<MissilePursueComponent> _pursuePool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<NextMissileComponent> _nextPool = default;
        readonly EcsPoolInject<MissileBackToCasterComponent> _missileBackPool;
        readonly EcsPoolInject<UnitCollisionEvent> _unitCollisionPool;
        public override MainEcsSystem Clone()
        {
            return new RunMissilePursueSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var missilePursueComp = ref _pursuePool.Value.Get(entity);
                ref var missileTransform = ref _transformPool.Value.Get(entity);
                ref var missileComp = ref _missilePool.Value.Get(entity);
                Vector3 currentDirection = missileTransform.Transform.forward;
                Vector3 targetDirection = (missileComp.TargetPosition - missileTransform.Transform.position).normalized;
                float angleToTarget = Vector3.Angle(currentDirection, targetDirection);
                if (angleToTarget > missilePursueComp.MaxTurnAngle)
                {
                    currentDirection = Vector3.RotateTowards(
                                              currentDirection,
                                              targetDirection,
                                              missilePursueComp.GetSpeedTurn(),
                                              0f
                                          );
                }
                else
                {
                    missilePursueComp.TurningTime = 0;
                }
                currentDirection.Normalize();
                currentDirection.y = Mathf.Clamp01(currentDirection.y);
                missileTransform.Transform.position += currentDirection * missileComp.Speed * Time.deltaTime;
                if (currentDirection != Vector3.zero)
                {
                    missileTransform.Transform.rotation = Quaternion.LookRotation(currentDirection);
                }
                missilePursueComp.MaxSecondsPursuit -= Time.deltaTime;
                if(missileComp.missile is CollisionMissileMB)
                {
                    var missileCollision = missileComp.missile as CollisionMissileMB;
                    if (missileCollision.Collision && missilePursueComp.isOneFlightTarget)
                    {
                        _pursuePool.Value.Del(entity);
                        _nextPool.Value.Add(entity);
                        continue;
                    }
                }
                if (missilePursueComp.MaxSecondsPursuit > 0) continue;
                if (_missileBackPool.Value.Has(entity)) continue;
                _pursuePool.Value.Del(entity);
                _nextPool.Value.Add(entity);
            }
        }
    }
}