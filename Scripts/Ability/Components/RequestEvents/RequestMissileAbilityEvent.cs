using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Client {
    struct RequestMissileAbilityEvent : IAbilityComponent , IZoneReference
    {
        [HideInInspector] public EcsPackedEntity OwnerEntity;
        [HideInInspector] public EcsPackedEntity AbilityPackedEntity;
        [SerializeReference] public List<IAbilityMissileComponent> Components; 
        public UsageValues UsageValue;
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve SpeedCurveValue;
        [ShowIf("UsageValue", UsageValues.Float)] public float SpeedValue;

        //todo CURVES
        public MissileMB missile;
        [HideInInspector] public float Speed;
        public Vector3 Offset;
        [HideInInspector] public string LayerNameTarget;
        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {
            
        }

        public void GetReference()
        {
            
        }

        public void Init()
        {
            
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            if (Components.Count == 0) return;
            //todo кол-во мислов
            ref var requestComp = ref world.GetPool<RequestMissileAbilityEvent>().Add(world.NewEntity());
            requestComp.OwnerEntity = world.PackEntity(ownerEntity);
            requestComp.AbilityPackedEntity = world.PackEntity(abilityEntity);
            requestComp.Components = new List<IAbilityMissileComponent>(Components);
            requestComp.missile = missile;
            
            requestComp.Offset = Offset;

            if(UsageValue == UsageValues.Float)
            {
                Speed = SpeedValue;
            }
            else if(UsageValue == UsageValues.Curve)
            {
                Speed = SpeedCurveValue.Evaluate(charge);
            }
            requestComp.Speed = Speed;
        }
    }
}