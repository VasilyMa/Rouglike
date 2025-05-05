using AbilitySystem;

using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Client {
    struct PushEffect : IAbilityEffect
    {
        [HideInInspector] public float PushForce;

        EcsPool<PushEffect> _pool;
        [HideInInspector]public EcsPackedEntity SenderEntity;
        public UsageValues UsageValue;
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve CurveValue;
        [ShowIf("UsageValue", UsageValues.Float)] public float PushForceValue;
        public void Invoke(int entity, int entitySender, EcsWorld world)
        {
            _pool = world.GetPool<PushEffect>();
            if (!_pool.Has(entity)) _pool.Add(entity);
            ref var poolComp = ref _pool.Get(entity);
            poolComp.PushForce = PushForce;
            poolComp.SenderEntity = world.PackEntity(entitySender);
        }
        public void Recalculate(float charge)
        {
            if(UsageValue == UsageValues.Curve)
            {
                PushForce = CurveValue.Evaluate(charge);
            }
            else
            {
                PushForce = PushForceValue;
            }
        }
    }
}