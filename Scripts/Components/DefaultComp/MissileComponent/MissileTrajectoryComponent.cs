using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct MissileTrajectoryComponent : IAbilityMissileComponent
    {
        public AnimationCurve curve;
        public Vector2 Scale;
        public float Speed;
        [HideInInspector] public float TimeMove;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Vector3 startForward;
        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var transfromComp = ref world.GetPool<TransformComponent>().Get(entity);
            ref var missileTrajectoryComp = ref world.GetPool<MissileTrajectoryComponent>().Add(entity);
            ref var missileComponent = ref world.GetPool<MissileComponent>().Get(entity);
            transfromComp.Transform.LookAt(missileComponent.TargetPosition);
            missileTrajectoryComp.Scale = Scale;
            missileTrajectoryComp.curve = new(curve.keys);
            missileTrajectoryComp.startPosition = transfromComp.Transform.position;
            missileTrajectoryComp.startForward = transfromComp.Transform.forward;
            missileTrajectoryComp.TimeMove = 0;
            missileTrajectoryComp.Speed = Speed;
        }
    }
}