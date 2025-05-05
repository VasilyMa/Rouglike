using System.Collections.Generic;
using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client 
{
    struct VisualEffectsComponent
    {
        public List<SourceParticle> SourceParticles;
        public DashParticle DashParticle;
        public void Init()
        {
            SourceParticles = new List<SourceParticle>();
        }
    }
}