using AbilitySystem;
using Client;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RequestVFXOnBody : IAbilityComponent
{
    public int IndexBody;

    public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
    { 
    }

    public void Init()
    {
    }

    public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
    {
        var physicsPool = world.GetPool<PhysicsUnitComponent>();
        if (!physicsPool.Has(ownerEntity)) return;
        ref var phycicsUnitComp = ref physicsPool.Get(ownerEntity);
        phycicsUnitComp.PhysicsUnitMB.ActiveVFXOnBody(IndexBody);
    }
}
