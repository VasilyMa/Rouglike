using AbilitySystem;
using Leopotam.EcsLite;

namespace Client {
    struct DelRotateComponent : IAbilityComponent
    {
        public void Init()
        {

        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            EcsPool<DelRotateComponent> pool = world.GetPool<DelRotateComponent>();
            if(!pool.Has(entityCaster)) pool.Add(entityCaster);
        }

        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {

        }
    }
}