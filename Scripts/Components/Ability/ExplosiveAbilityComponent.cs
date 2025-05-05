using AbilitySystem;

using Leopotam.EcsLite;

using UnityEngine;

namespace Client 
{
    struct ExplosiveAbilityComponent : IAbilityComponent
    {
        public SourceParticle ExpolosiveEffect;
        public float DamageValue;
        public float Radius;
        public LayerMask LayerMask;
        public int OwnerEntity;
        public int AbilityEntity;
        public int Level;
        public void Init()
        {

        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {

        }

        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {

        }
    }
}