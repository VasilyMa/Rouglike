using AbilitySystem;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct MissileTrailComponent : IAbilityMissileComponent
    {
        public TrailComponent trail;
        public ResolveTrailComponent resolveTrail;
        [SerializeReference] public IDestroyTrail destroyTrail;

        public void Invoke(int entity, EcsWorld world, float charge)
        {
            var entityTrail = world.NewEntity();
            ref var missileComp = ref world.GetPool<MissileComponent>().Get(entity);
            trail.Invoke(entityTrail, entity, world, missileComp.LayerNameTarget);
            resolveTrail.Invoke(entityTrail, world);
            destroyTrail.Invoke(entityTrail, world);
            world.GetPool<MissileTrailComponent>().Add(entity);
        }
    }
}