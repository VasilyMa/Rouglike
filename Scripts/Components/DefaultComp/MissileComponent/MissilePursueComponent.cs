using AbilitySystem;
using Leopotam.EcsLite;
using Unity.VisualScripting;
using UnityEngine;

namespace Client {
    struct MissilePursueComponent : IAbilityMissileComponent
    {
        public float MaxSecondsPursuit;
        public float MaxTurnAngle;
        public bool NotLookInTarget;
        public bool isOneFlightTarget;
        [Range(0,720)]
        public float MaxSpeedTurn;
        public AnimationCurve ChangeSpeedTurn;
        [HideInInspector] public float TurningTime;
        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var missileComp = ref world.GetPool<MissileComponent>().Get(entity);
            ref var missilePursueComp = ref world.GetPool<MissilePursueComponent>().Add(entity);
            missilePursueComp.MaxSecondsPursuit = MaxSecondsPursuit;
            missilePursueComp.MaxSpeedTurn = MaxSpeedTurn;
            missilePursueComp.MaxTurnAngle = MaxTurnAngle;
            missilePursueComp.ChangeSpeedTurn = ChangeSpeedTurn;
            missilePursueComp.TurningTime = 0;
            missilePursueComp.isOneFlightTarget = isOneFlightTarget;
            ref var transformMissileComp = ref world.GetPool<TransformComponent>().Get(entity);
            if(!NotLookInTarget) transformMissileComp.Transform.LookAt(missileComp.TargetPosition);
        }
        public float GetSpeedTurn()
        {
            TurningTime +=Time.deltaTime;
            var fo = ChangeSpeedTurn.Evaluate(TurningTime);
            return ChangeSpeedTurn.Evaluate(TurningTime) * MaxSpeedTurn * Mathf.Deg2Rad * Time.deltaTime;
        }
    }
}