using AbilitySystem;

using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client {
    struct MissileToPointComponent : IAbilityMissileComponent
    {
        [HideInInspector] public Vector3 TargetPoint;
        public bool isRandomPoint;
        [ShowIf("isRandomPoint")] public float MinRange;
        [ShowIf("isRandomPoint")] public float MaxRange;

        public void Init()
        {

        }

        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var missileToPointComp = ref world.GetPool<MissileToPointComponent>().Add(entity);
            ref var missileComp = ref world.GetPool<MissileComponent>().Get(entity);
            missileToPointComp.TargetPoint = missileComp.TargetPosition;
            if (isRandomPoint)
            {
                ref var transformMissileComp = ref world.GetPool<TransformComponent>().Get(entity);
                missileToPointComp.TargetPoint = RandomPointGenerator.GetRandomPoint(transformMissileComp.Transform.position, MinRange, MaxRange) + missileComp.Offset;
                transformMissileComp.Transform.LookAt(missileToPointComp.TargetPoint);
            }
        }

        public void Dispose(int entityCaster, EcsWorld world)
        {

        }

    }
}