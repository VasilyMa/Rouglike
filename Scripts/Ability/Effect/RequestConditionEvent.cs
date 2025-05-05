using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RequestConditionEvent : IAbilityEffect
{
    public int ExtensionsPointPoint;
    public Condition Condition;
    [HideInInspector] public EcsPackedEntity entityTarget;
    public void Invoke(int entity, int entitySender, EcsWorld world)
    {
        var _requestConditionPool = world.GetPool<RequestConditionEvent>();
        ref var requestConditionComp = ref _requestConditionPool.Add(world.NewEntity());
        requestConditionComp.ExtensionsPointPoint = ExtensionsPointPoint + 1;
        requestConditionComp.Condition = Condition;
        requestConditionComp.entityTarget = world.PackEntity(entity);
    }

    public void Recalculate(float charge)
    {
        
    }
}
