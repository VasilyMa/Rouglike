using UnityEngine;
using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;

namespace Client {
    struct RequestChangeAnimationEvent : IAbilityComponent
    {
        [HideInInspector] public EcsPackedEntity TargetPackedEntity;
        public AnimationTypes AnimationType;
        public bool RootMotion;
        public bool IsUniqueAnimation;
        [ShowIf("IsUniqueAnimation",true)]public AnimationClip UniqueAnimation;
        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
            
        }

        public void Init()
        {
            
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestComp = ref world.GetPool<RequestChangeAnimationEvent>().Add(world.NewEntity());
            requestComp.TargetPackedEntity = world.PackEntity(ownerEntity);
            requestComp.AnimationType = AnimationType;
            requestComp.RootMotion = RootMotion;
            requestComp.IsUniqueAnimation = IsUniqueAnimation;
            requestComp.UniqueAnimation = UniqueAnimation;
        }
    }
}