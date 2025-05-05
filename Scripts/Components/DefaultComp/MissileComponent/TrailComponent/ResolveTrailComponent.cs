using AbilitySystem;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    [System.Serializable]
    struct ResolveTrailComponent
    {
        public float TimeToResolve;
        [SerializeReference] private List<IAbilityEffect> listResolve;
        [HideInInspector] public float TimerToResolve;
        public void Invoke(int entityTrail,EcsWorld world)
        {
            ref var ResolveTrailComp = ref world.GetPool<ResolveTrailComponent>().Add(entityTrail);
            ref var ResolveBlockComp = ref world.GetPool<ResolveBlockComponent>().Add(entityTrail);
            ResolveBlockComp.Components = new(listResolve);
            foreach(var component in ResolveBlockComp.Components)
            {
                component.Recalculate(1f);
            }
            ResolveTrailComp.TimeToResolve = TimeToResolve;
            ResolveTrailComp.TimerToResolve = TimerToResolve;
        }
    }
}
