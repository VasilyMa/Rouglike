using AbilitySystem;
using Leopotam.EcsLite;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Client {
    struct RequestAttackRoomEvent : IAbilityComponent
    {
        public int IndexAttack;
        public float TimeToResolve;
        public float LifeTime;
        public float Delay;
        [SerializeReference] public List<IAbilityEffect> Components;
        public void Dispose(int entityCaster, int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
        }

        public void Invoke(int ownerEntity, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var requestAttackRoomPool = ref world.GetPool<RequestAttackRoomEvent>().Add(world.NewEntity());
            requestAttackRoomPool.IndexAttack = IndexAttack;
            requestAttackRoomPool.Components = Components;
            requestAttackRoomPool.TimeToResolve = TimeToResolve;
            requestAttackRoomPool.LifeTime = LifeTime;
            requestAttackRoomPool.Delay = Delay;
        }
    }
}