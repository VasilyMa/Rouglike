using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
namespace Client {
    struct StartCooldownAbilityEvent : IAbilityComponent
    {
        [HideInInspector] public bool NormalCoolDown;
        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {
            if(!world.GetPool<StartCooldownAbilityEvent>().Has(abilityEntity)) 
            {
                ref var startCoolDownComp = ref world.GetPool<StartCooldownAbilityEvent>().Add(abilityEntity);
                startCoolDownComp.NormalCoolDown = false;
            }
        }

        public void GetReference()
        {
            
        }

        public void Init()
        {
            
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            if(!world.GetPool<StartCooldownAbilityEvent>().Has(abilityEntity)) 
            {
                ref var startCoolDownComp = ref world.GetPool<StartCooldownAbilityEvent>().Add(abilityEntity);
                startCoolDownComp.NormalCoolDown = true;
            }
            
        }
    }
}