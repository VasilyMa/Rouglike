using AbilitySystem;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client
{
    struct TiredComponent : IAbilityComponent
    {
        public float Duration;
        EcsPool<TiredComponent> _pool;
        [HideInInspector]public bool IsTired;
        public void Init()
        {
        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            _pool = world.GetPool<TiredComponent>();
            if (!_pool.Has(entityCaster)) _pool.Add(entityCaster);
            ref var poolComp = ref _pool.Get(entityCaster);
            poolComp.Duration = Duration;
            poolComp.IsTired = false;
        }

        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
        }
    }
}