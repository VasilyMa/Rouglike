
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public struct DestroyTrailFromTailComponent : IDestroyTrail
    {
        public float DelayDestroy;
        public float TimeOfDestroyBounds;
        [HideInInspector] public float TimerDestoyBounds;
        public void Invoke(int entityTrail,EcsWorld world)
        {
            ref var destroyTail = ref world.GetPool<DestroyTrailFromTailComponent>().Add(entityTrail);
            destroyTail.DelayDestroy = DelayDestroy;
            destroyTail.TimeOfDestroyBounds = TimeOfDestroyBounds;
            destroyTail.TimerDestoyBounds = TimerDestoyBounds;
        }
    }
    public interface IDestroyTrail
    {
        public void Invoke(int entityTrail, EcsWorld world);
    }
}
