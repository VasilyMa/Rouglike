using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct RequestLockRotationEvent : IAbilityComponent
    {
        [HideInInspector] public EcsPackedEntity TargetPackedEntity;

        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {
            
        }

        public void Init()
        {
            
        }
        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestComp = ref world.GetPool<RequestLockRotationEvent>().Add(world.NewEntity());
            requestComp.TargetPackedEntity = world.PackEntity(ownerEntity);
        }
    }
}