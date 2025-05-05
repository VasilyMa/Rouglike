using AbilitySystem;
using Leopotam.EcsLite;

namespace Client {
    struct ShakeCameraEvent : IAbilityComponent, IAbilityEffect
    {
        public void Dispose(int entityCaster,int abilityEntity, EcsWorld world)
        {
        }

        public void Init()
        {
        }

        public void Invoke(int entityCaster, EcsWorld world)
        {
            EcsPool<ShakeCameraEvent> _shakePool = world.GetPool<ShakeCameraEvent>();
            _shakePool.Add(world.NewEntity());
        }

        public void Invoke(int entity, int entitySender, EcsWorld world, float charge = 1)
        {
            EcsPool<ShakeCameraEvent> _shakePool = world.GetPool<ShakeCameraEvent>();
            _shakePool.Add(world.NewEntity());
        }

        public void Invoke(int entity, int entitySender, EcsWorld world)
        {
            
        }

        public void Recalculate(float charge)
        {
            
        }
    }
}