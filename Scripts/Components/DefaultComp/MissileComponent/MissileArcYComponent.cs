using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client {
    struct MissileArcYComponent : IAbilityMissileComponent
    {
        public float maxHeight ; 

        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Vector3 targetPosition;
        [HideInInspector] public Vector3 auxPosition;
        [HideInInspector] public float travelTime; 
        [HideInInspector] public float elapsedTime;
        public bool isRandomPoint;
        [ShowIf("isRandomPoint")] public float MinRange;
        [ShowIf("isRandomPoint")] public float MaxRange;
        public TelegraphyOfMissileTargetComponent TelegraphyOfMissileTarget;
        public void Invoke(int entity,EcsWorld world, float charge)
        {
            ref var missileArcYComp = ref world.GetPool<MissileArcYComponent>().Add(entity);
            ref var transformMissileComp = ref world.GetPool<TransformComponent>().Get(entity);
            ref var missileComponent = ref world.GetPool<MissileComponent>().Get(entity);
            missileArcYComp.targetPosition = missileComponent.TargetPosition;
            if (isRandomPoint)
                missileArcYComp.targetPosition = RandomPointGenerator.GetRandomPoint(transformMissileComp.Transform.position, MinRange, MaxRange);
            missileArcYComp.startPosition = transformMissileComp.Transform.TransformPoint(missileComponent.Offset);
            float distance = Vector3.Distance(missileArcYComp.startPosition, missileArcYComp.targetPosition);
            missileArcYComp.auxPosition = missileArcYComp.startPosition + (missileArcYComp.targetPosition - missileArcYComp.startPosition) * 0.5f + Vector3.up * maxHeight;
            missileArcYComp.travelTime = distance / missileComponent.Speed;
            missileArcYComp.elapsedTime = 0;
            missileArcYComp.maxHeight = maxHeight;
            TelegraphyOfMissileTarget.Invoke(world, missileArcYComp.targetPosition);
        }
    }
}