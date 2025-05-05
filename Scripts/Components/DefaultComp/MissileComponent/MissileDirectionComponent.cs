using AbilitySystem;
using Sirenix.OdinInspector;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct MissileDirectionComponent : IAbilityMissileComponent
    {
        [HideInInspector] public float Distance;
        [HideInInspector] public Vector3 Direction;
        [HideInInspector] public float PassedWay;
        public UsageValues UsageValue;
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve DistanceCurveValue;
        [ShowIf("UsageValue", UsageValues.Float)] public float DistanceValue;
        public float AngleOfDeviation;
        public void Init()
        {

        }

        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var missileDirectionComp = ref world.GetPool<MissileDirectionComponent>().Add(entity);
            
            ref var transformComp = ref world.GetPool<TransformComponent>().Get(entity);
            //ref var transformCompCaster = ref world.GetPool<TransformComponent>().Get(entityCaster);
            //transformComp.Transform.localEulerAngles = transformCompCaster.Transform.rotation.eulerAngles;
            if(UsageValue == UsageValues.Float)
            {
                Distance = DistanceValue;
            }
            else if(UsageValue == UsageValues.Curve)
            {
                Distance = DistanceCurveValue.Evaluate(charge);
            }
            missileDirectionComp.Distance = Distance;

            Vector3 originalVector = transformComp.Transform.forward; // �������� ������
            Quaternion rotation = Quaternion.Euler(0, AngleOfDeviation, 0);
            Vector3 rotatedVector = rotation * originalVector;
            missileDirectionComp.Direction = rotatedVector;
        }

        public void Dispose(int entityCaster, EcsWorld world)
        {

        }

    }
}