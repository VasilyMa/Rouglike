using AbilitySystem;
using UnityEngine;
using Leopotam.EcsLite;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Client 
{
    struct DamageEffect : IAbilityEffect, IConditionResolve
    {
        [HideInInspector] public float DamageFinalValue;
        [HideInInspector] public float AdditiveModifiers;
        [HideInInspector] public float MultiplicativeModifiers;
        public UsageValues UsageValue;
        [ShowIf("UsageValue",UsageValues.Curve)] public AnimationCurve CurveValue;
        [ShowIf("UsageValue", UsageValues.Float)] public float DamageValue;
        public EcsPackedEntity SenderPackedEntity;
        public EcsPackedEntity TargetPackedEntity;
        [HideInInspector] public bool IsConditionDamage;

        public void Invoke(int entity, int senderEntity, EcsWorld world)
        {
            ref var poolComp = ref world.GetPool<DamageEffect>().Add(world.NewEntity());
            poolComp.DamageFinalValue = DamageFinalValue;
            poolComp.SenderPackedEntity = world.PackEntity(senderEntity);
            poolComp.TargetPackedEntity = world.PackEntity(entity);
            poolComp.IsConditionDamage = IsConditionDamage;
        }

        public void InvokeResolve(int entityCondition, int entityOwner, EcsWorld world)
        {
            IsConditionDamage = true;
            Invoke(entityOwner, entityOwner, world);
        }

        public void Recalculate(float charge)
        {
            if(UsageValue == UsageValues.Curve)
            {
                DamageFinalValue = CurveValue.Evaluate(charge);
            }
            else
            {
                DamageFinalValue = DamageValue;
            }
            DamageFinalValue += AdditiveModifiers;
            if(MultiplicativeModifiers != 0) DamageFinalValue *= MultiplicativeModifiers;
        }
    }
    public enum UsageValues{
        Float, Curve, Vector3
    }
}
