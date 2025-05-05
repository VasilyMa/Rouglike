using AbilitySystem;
using Client;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageDamageEffect : IAbilityEffect,IConditionResolve
{
    [HideInInspector] public float PercentageDamageFinal;
    [HideInInspector] public float MaxValueDamageFinalFinal;

    public AnimationCurve PrecentageDamageValue;
    public AnimationCurve MaxValueDamageCurve;

    public EcsPackedEntity SenderPackedEntity;
    public EcsPackedEntity TargetPackedEntity;
    public void Invoke(int entity, int entitySender, EcsWorld world)
    {
        var _healthPool = world.GetPool<HealthComponent>();
        if (!_healthPool.Has(entity)) return;
        ref var healthComp = ref _healthPool.Get(entity);
        var DamageFinalValue = Mathf.Clamp(healthComp.MaxValue * PercentageDamageFinal, 0, MaxValueDamageFinalFinal);
        ref var poolComp = ref world.GetPool<DamageEffect>().Add(world.NewEntity());
        poolComp.DamageFinalValue = DamageFinalValue;
        poolComp.SenderPackedEntity = world.PackEntity(entitySender);
        poolComp.TargetPackedEntity = world.PackEntity(entity);
    }

    public void InvokeResolve(int entityCondition, int entityOwner, EcsWorld world)
    {
        Invoke(entityOwner, entityOwner, world);
    }

    public void Recalculate(float charge)
    {
        MaxValueDamageFinalFinal = MaxValueDamageCurve.Evaluate(charge);
        PercentageDamageFinal = PrecentageDamageValue.Evaluate(charge);
    }
}