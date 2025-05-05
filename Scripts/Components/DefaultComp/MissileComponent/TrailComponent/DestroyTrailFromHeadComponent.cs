using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public struct DestroyTrailFromHeadComponent : IDestroyTrail
    {
        public float DelayDestroy;
        public float TimeOfDestroyBounds;
        [HideInInspector] public float TimerDestoyBounds;
        public void Invoke(int entityTrail, EcsWorld world)
        {
            ref var destroyTail = ref world.GetPool<DestroyTrailFromHeadComponent>().Add(entityTrail);
            destroyTail.DelayDestroy = DelayDestroy;
            destroyTail.TimeOfDestroyBounds = TimeOfDestroyBounds;
            destroyTail.TimerDestoyBounds = TimerDestoyBounds;
        }
    }
}
