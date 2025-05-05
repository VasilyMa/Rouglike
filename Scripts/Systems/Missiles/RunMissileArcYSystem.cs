using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RunMissileArcYSystem : MainEcsSystem 
    {
        readonly EcsFilterInject<Inc<MissileComponent, MissileArcYComponent>> _filter;
        readonly EcsPoolInject<MissileComponent> _missilePool;
        readonly EcsPoolInject<MissileArcYComponent> _missileArcPool;
        readonly EcsPoolInject<NextMissileComponent> _nextMissilePool;
        readonly EcsPoolInject<TransformComponent> _transformPool;

        public override MainEcsSystem Clone()
        {
            return new RunMissileArcYSystem();
        }

        public override void Run (IEcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var missileArcComp = ref _missileArcPool.Value.Get(entity);
                ref var missileComp = ref _missilePool.Value.Get(entity);
                ref var transformMissileComp = ref _transformPool.Value.Get(entity);
                if (missileArcComp.elapsedTime <= missileArcComp.travelTime)
                {
                    missileArcComp.elapsedTime += Time.deltaTime;
                    float t = missileArcComp.elapsedTime / missileArcComp.travelTime;
                    /*Vector3 horizontalPosition = Vector3.Lerp(missileArcComp.startPosition, missileArcComp.targetPosition, t);
                    float height = Mathf.Sin(t * Mathf.PI) * missileArcComp.maxHeight;*/
                    var newPosition = Bezier(missileArcComp.startPosition, missileArcComp.auxPosition, missileArcComp.targetPosition, t);
                    transformMissileComp.Transform.rotation = Quaternion.LookRotation(newPosition - transformMissileComp.Transform.position); ;
                    transformMissileComp.Transform.position = newPosition;
                }
                else
                {
                    transformMissileComp.Transform.up = Vector3.up;
                    _missileArcPool.Value.Del(entity);
                    _nextMissilePool.Value.Add(entity);
                }
            }
        }

        private Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 p12 = Vector3.Lerp(p1, p2, t);
            Vector3 p23 = Vector3.Lerp(p2, p3, t);
            return  Vector3.Lerp(p12, p23, t);

        }
    }
}