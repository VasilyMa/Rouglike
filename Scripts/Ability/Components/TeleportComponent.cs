using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    struct TeleportComponent : IAbilityComponent
    {
        [Range(0, 10)] public float TeleportRadiusInRoom;
        EcsPool<TeleportComponent> _pool;
        [HideInInspector]public bool IsTeleporting;
        [HideInInspector]public Vector3 RandomPos;

        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
        }
        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            _pool = world.GetPool<TeleportComponent>();
            if (!_pool.Has(entityCaster)) _pool.Add(entityCaster);
            ref var poolComp = ref _pool.Get(entityCaster);
            poolComp.TeleportRadiusInRoom = TeleportRadiusInRoom;
            poolComp.IsTeleporting = IsTeleporting = false;
        }
    }
}