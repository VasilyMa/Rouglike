using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct HitEffect : IAbilityEffect, IConditionResolve
    {
        public SourceParticle SourceParticle;
        [HideInInspector] public List<SourceParticle> ParticlesToPlay;
        [Range(0,3)] public float OffsetZ;
        [HideInInspector]public int EntitySender;
        EcsPool<HitEffect> _pool;
        public void Invoke(int entity, int entitySender, EcsWorld world)
        {
            _pool = world.GetPool<HitEffect>();
            if (!_pool.Has(entity)) _pool.Add(entity).ParticlesToPlay = new List<SourceParticle>();
            ref var poolComp = ref _pool.Get(entity);
            poolComp.ParticlesToPlay.Add(SourceParticle);
            poolComp.OffsetZ = OffsetZ;
            poolComp.EntitySender = entitySender;

            /*foreach (var particle in poolComp.ParticlesToPlay)
            {
                GameState.Instance.CreatePool(particle.gameObject, particle.name);
            }
            */
        }

        public void InvokeResolve(int entityCondition, int entityOwner, EcsWorld world)
        {
            Invoke(entityOwner, entityOwner, world);
        }

        public void Recalculate(float charge)
        {
            
        }
    }
}