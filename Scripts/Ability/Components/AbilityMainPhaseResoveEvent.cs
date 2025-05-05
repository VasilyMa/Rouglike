using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Client {
    struct AbilityMainPhaseResolveEvent : IAbilityComponent
    {
        [HideInInspector] public ModifierTags AbilityTags;
        // add your data here.
        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {
            
        }

        public void Init()
        {
            
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            if(world.GetPool<PlayerAbilityComponent>().Has(abilityEntity))
            {
                ref var resolveComp = ref world.GetPool<AbilityMainPhaseResolveEvent>().Add(world.NewEntity());
                resolveComp.AbilityTags = world.GetPool<AbilityComponent>().Get(abilityEntity).Ability.ModifierTags;
            }
        }
    }
}