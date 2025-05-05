using AbilitySystem;
using DG.Tweening;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Client {
    struct MissileBackToCasterComponent : IAbilityMissileComponent
    {
        public float DistanceDifference;
        public float MaxTimeOfPursuit;
        [HideInInspector] public EcsPackedEntity oldEntityTarget;
        [HideInInspector] public Vector3 oldTargetPosition;
        public void Invoke(int entity,EcsWorld world, float charge)
        {
            ref var missileBackComponent = ref world.GetPool<MissileBackToCasterComponent>().Add(entity);
            ref var missileComp = ref world.GetPool<MissileComponent>().Get(entity);
            ref var missilePursueComp = ref world.GetPool<MissilePursueComponent>().Add(entity);

            missileBackComponent.DistanceDifference = DistanceDifference;
            missilePursueComp.MaxSecondsPursuit = MaxTimeOfPursuit;
            missilePursueComp.MaxTurnAngle = 0;
            missilePursueComp.MaxSpeedTurn = 720;
            missilePursueComp.ChangeSpeedTurn = new(new Keyframe(0f, 1f), new Keyframe(1f, 1f));
            missilePursueComp.TurningTime = 0;

            missileBackComponent.oldTargetPosition = missileComp.TargetPosition;
            missileComp.TargetPosition = missileComp.CasterPosition;
            var targetPool = world.GetPool<TargetMissileComponent>();
            if(targetPool.Has(entity))
            {
                ref var targetComp = ref targetPool.Get(entity);
                missileBackComponent.oldEntityTarget = targetComp.EntityTarget;
                var casterPool = world.GetPool<CasterMissileComponent>();
                if(casterPool.Has(entity))
                {
                    ref var casterComp = ref casterPool.Get(entity);
                    targetComp.EntityTarget = casterComp.EntityCaster;
                }
                else
                {
                    targetPool.Del(entity);
                }
            }
            else
            {
                ref var targetComp = ref targetPool.Add(entity);
                var casterPool = world.GetPool<CasterMissileComponent>();
                ref var casterComp = ref casterPool.Get(entity);
                targetComp.EntityTarget = casterComp.EntityCaster;
            }

                ref var transformMissileComp = ref world.GetPool<TransformComponent>().Get(entity);
            transformMissileComp.Transform.LookAt(missileComp.TargetPosition);
        }
    }
}