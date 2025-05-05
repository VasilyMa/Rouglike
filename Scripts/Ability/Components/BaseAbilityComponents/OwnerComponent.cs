using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;


namespace Client {
    struct OwnerComponent
    {
        [HideInInspector] public EcsPackedEntity OwnerEntity;
        // public void Init(int entity, EcsWorld world, int entityOwner)
        // {
        //     ref var ownerComp = ref world.GetPool<OwnerComponent>().Add(entity);
        //     ownerComp.OwnerEntity = world.PackEntity(entityOwner);
        // }
    }
}