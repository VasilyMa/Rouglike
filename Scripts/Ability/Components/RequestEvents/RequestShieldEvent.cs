using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client {
    struct RequestShieldEvent : IAbilityComponent
    {
        public SourceParticle SourceParticle;
        [Range(0, 3000)] public float DamageProtection;
        [Range(0, 300)] public float Duration;
        [HideInInspector] public EcsPackedEntity TargetPackedEntity;

        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {
        }

        public void Init()
        {
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestComp = ref world.GetPool<RequestShieldEvent>().Add(world.NewEntity());
            requestComp.Duration = Duration;
            requestComp.DamageProtection = DamageProtection;
            requestComp.SourceParticle = SourceParticle;
            requestComp.TargetPackedEntity = world.PackEntity(ownerEntity);
        }
    }
}