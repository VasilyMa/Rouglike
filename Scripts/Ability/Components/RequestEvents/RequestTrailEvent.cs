using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Client {
    struct RequestTrailEvent : IAbilityComponent
    {
        public TrailComponent trail;
        public ResolveTrailComponent resolveTrail;
        [SerializeReference] public IDestroyTrail destroyTrail;
        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            var entityTrail = world.NewEntity();
            var colliderPool = world.GetPool<ColliderComponent>();
            var transformPool = world.GetPool<TransformComponent>();
            if(colliderPool.Has(ownerEntity) && transformPool.Has(ownerEntity))
            {
                if(world.GetPool<PlayerComponent>().Has(ownerEntity))
                    trail.Invoke(entityTrail, ownerEntity, world, "Enemy");
                else
                    trail.Invoke(entityTrail, ownerEntity, world, "Player");
                resolveTrail.Invoke(entityTrail, world);
                destroyTrail.Invoke(entityTrail, world);
            }

        }
    }
}