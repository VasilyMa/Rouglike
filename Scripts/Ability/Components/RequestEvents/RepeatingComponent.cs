using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct RepeatingComponent : IAbilityComponent
    {
        public int Count;
        public float Delay;
        [SerializeReference] public List<IAbilityComponent> FXComponents;
        [HideInInspector] public  float TimeDelay;
        [HideInInspector] public  float Charge;
        [HideInInspector] public EcsPackedEntity OwnerEntity;
        [HideInInspector] public EcsPackedEntity AbilityEntity;

        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {

        }

        public void Init()
        {

        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var repeatingComp = ref world.GetPool<RepeatingComponent>().Add(world.NewEntity());
            repeatingComp.Count = Count;
            repeatingComp.Delay = Delay;
            repeatingComp.FXComponents = new(FXComponents);
            repeatingComp.TimeDelay = 0;
            repeatingComp.OwnerEntity = world.PackEntity(ownerEntity);
            repeatingComp.AbilityEntity = world.PackEntity(abilityEntity);
            repeatingComp.Charge = charge;
        }
    }
}