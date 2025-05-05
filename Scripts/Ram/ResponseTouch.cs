using AbilitySystem;
using Leopotam.EcsLite;

namespace Client {
    struct ResponseTouch : IAbilityComponent
    {
        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            EcsPool<ResponseTouch> _pool = world.GetPool<ResponseTouch>();
            if (!_pool.Has(entityCaster)) _pool.Add(entityCaster);
        }
    }
    struct DelResponseTouch : IAbilityComponent
    {
        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {
        }

        public void Init()
        {}

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            EcsPool<ResponseTouch> _pool=world.GetPool<ResponseTouch>();
            if(_pool.Has(entityCaster)) _pool.Del(entityCaster);
        }
    }
    struct LockAfterCast { }
}