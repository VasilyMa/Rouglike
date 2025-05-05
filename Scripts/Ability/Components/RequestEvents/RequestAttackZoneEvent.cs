using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Client {
    struct RequestAttackZoneEvent : IAbilityComponent , IZoneReference
    {
        [HideInInspector] public EcsPackedEntity TargetPackedEntity;
        [HideInInspector] public EcsPackedEntity AbilityEntity;
        [HideInInspector] public Vector3 Size;
        
        
        public Mesh AttackMesh;
        public UsageValues UsageValue;
        
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve CurveXValue;
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve CurveZValue;
        [ShowIf("UsageValue", UsageValues.Vector3)] public Vector3 SizeValue;
        public float DisableTime;

        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {

        }

        public void GetReference()
        {
            
        }

        public void Init()
        {

        }

        public void Invoke(int ownerEntity,int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestComp = ref world.GetPool<RequestAttackZoneEvent>().Add(world.NewEntity());
            requestComp.TargetPackedEntity = world.PackEntity(ownerEntity);
            requestComp.AttackMesh = AttackMesh;
            requestComp.AbilityEntity = world.PackEntity(abilityEntity);

            Vector3 size = Vector3.one;
            if(UsageValue == UsageValues.Curve)
            {
                size = new Vector3(CurveXValue.Evaluate(charge), 1, CurveZValue.Evaluate(charge));
            }
            else if(UsageValue == UsageValues.Vector3)
            {
                size = SizeValue;
            }
            else
            {
                size = Vector3.one;
            }

            requestComp.Size = size;
            requestComp.DisableTime = DisableTime;
        }

    }
}