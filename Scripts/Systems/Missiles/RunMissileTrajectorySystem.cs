using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Client {
    sealed class RunMissileTrajectorySystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<MissileComponent, MissileTrajectoryComponent>> _filter = default;
        readonly EcsPoolInject<MissileTrajectoryComponent> _missileTrajectiryPool;
        readonly EcsPoolInject<MissileComponent> _missilePool;
        readonly EcsPoolInject<TransformComponent> _transformPool;
        readonly EcsPoolInject<MissilePermanentComponent> _permanentPool = default;
        readonly EcsPoolInject<NextMissileComponent> _nextPool = default;

        public override MainEcsSystem Clone()
        {
            return new RunMissileTrajectorySystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach(var entity in _filter.Value)
            {
                ref var missileTrajectoryComp = ref _missileTrajectiryPool.Value.Get(entity);
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var transfromMissile = ref _transformPool.Value.Get(entity);
                var duration = missileTrajectoryComp.curve.keys[missileTrajectoryComp.curve.length - 1].time;
                if (missileTrajectoryComp.TimeMove < duration)
                {
                    missileTrajectoryComp.TimeMove += Time.deltaTime * missileTrajectoryComp.Speed;
                    Vector3 perpendicularDirection = Vector3.Cross(missileTrajectoryComp.startForward, Vector3.up).normalized;
                    Vector3 newPositionX = perpendicularDirection * missileTrajectoryComp.curve.Evaluate(missileTrajectoryComp.TimeMove) * missileTrajectoryComp.Scale.y;
                    Vector3 newPositionZ = missileTrajectoryComp.TimeMove * missileTrajectoryComp.startForward * missileTrajectoryComp.Scale.x;
                    var newPosition = newPositionX + newPositionZ + missileTrajectoryComp.startPosition;
                    newPosition.y = missileComp.Offset.y;
                    var missileMoveDir = newPosition  - transfromMissile.Transform.position;
                    transfromMissile.Transform.position = newPosition;
                    transfromMissile.Transform.rotation = Quaternion.LookRotation(missileMoveDir);
                }
                else
                {

                    _missileTrajectiryPool.Value.Del(entity);
                    _nextPool.Value.Add(entity);
                }

            }
        }
    }
}