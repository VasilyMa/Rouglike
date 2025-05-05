using AbilitySystem;
using UnityEngine;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;

namespace Client {
    struct MissilePermanentComponent : IAbilityMissileComponent
    {
        [HideInInspector] public float Radius;
        public UsageValues UsageValueForRadius;
        [ShowIf("UsageValueForRadius",UsageValues.Curve)] public AnimationCurve RadiusCurveValue;
        [ShowIf("UsageValueForRadius", UsageValues.Float)] public float RadiusValue;

        [HideInInspector]public float Duration;
        public UsageValues UsageValueForDuration;
        [ShowIf("UsageValueForDuration",UsageValues.Curve)] public AnimationCurve DurationCurveValue;
        [ShowIf("UsageValueForDuration", UsageValues.Float)] public float DurationValue;

        [HideInInspector] public float TimeToResolve;
        public UsageValues UsageValueForTimeToResolve;
        [ShowIf("UsageValueForTimeToResolve",UsageValues.Curve)] public AnimationCurve TimeToResolveCurveValue;
        [ShowIf("UsageValueForTimeToResolve", UsageValues.Float)] public float TimeToResolveValue;
        public bool isRandomPoint;
        [ShowIf("isRandomPoint")] public float MinRange;
        [ShowIf("isRandomPoint")] public float MaxRange;


        [HideInInspector] public float _timeToReoslve;

        public void Init()
        {

        }

        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var permanentComp = ref world.GetPool<MissilePermanentComponent>().Add(entity);
            if(UsageValueForDuration == UsageValues.Float)
            {
                Duration = DurationValue;
            }
            else if(UsageValueForDuration == UsageValues.Curve)
            {
                Duration = DurationCurveValue.Evaluate(charge);
            }

            if(UsageValueForRadius == UsageValues.Float)
            {
                Radius = RadiusValue;
            }
            else if(UsageValueForRadius == UsageValues.Curve)
            {
                Radius = RadiusCurveValue.Evaluate(charge);
            }

            if(UsageValueForTimeToResolve == UsageValues.Float)
            {
                TimeToResolve = TimeToResolveValue;
            }
            else if(UsageValueForTimeToResolve == UsageValues.Curve)
            {
                TimeToResolve = TimeToResolveCurveValue.Evaluate(charge);
            }

            permanentComp.Duration = Duration;
            permanentComp.TimeToResolve = TimeToResolve;
            permanentComp.Radius = Radius;
            if (isRandomPoint)
            {
                ref var transformMissileComp = ref world.GetPool<TransformComponent>().Get(entity);
                var randomPoint = RandomPointGenerator.GetRandomPoint(transformMissileComp.Transform.position, MinRange, MaxRange);
                transformMissileComp.Transform.position = randomPoint;
            }
        }

        public void Dispose(int entityCaster, EcsWorld world)
        {

        }

    }
}