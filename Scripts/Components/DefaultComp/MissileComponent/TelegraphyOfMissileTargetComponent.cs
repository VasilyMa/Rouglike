using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Client {
    [System.Serializable]
    public struct TelegraphyOfMissileTargetComponent
    {
        public bool isTelegraphing;
        [ShowIf("isTelegraphing")] public NonCollisionMissileMB TelegraphingMissleMB;
        [HideInInspector] public NonCollisionMissileMB CreatedObject;
        [ShowIf("isTelegraphing")] public float LifeTime;
        [ShowIf("isTelegraphing")] public float TimeBeforeInvoke;
        [HideInInspector] public Vector3 Position;
        public void Invoke(EcsWorld World,Vector3 Position)
        {
            if (!isTelegraphing) return;
            if(TelegraphingMissleMB is null)
            {
                
                return;
            }
            var telegraphingOfMissleTargetPool = World.GetPool<TelegraphyOfMissileTargetComponent>();
            ref var telegraphingOfMissleTargetComp = ref telegraphingOfMissleTargetPool.Add(World.NewEntity());
            telegraphingOfMissleTargetComp.LifeTime = LifeTime;
            telegraphingOfMissleTargetComp.TimeBeforeInvoke = TimeBeforeInvoke;
            telegraphingOfMissleTargetComp.TelegraphingMissleMB = TelegraphingMissleMB;
            telegraphingOfMissleTargetComp.Position = Position;
        }

    }
}