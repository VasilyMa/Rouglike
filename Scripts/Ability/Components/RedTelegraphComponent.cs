using AbilitySystem;

using Leopotam.EcsLite;

namespace Client 
{
    struct RedTelegraphComponent : IAbilityComponent
    {
        public void Init()
        {

        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var telegoUnitComp = ref world.GetPool<TelegraphingUnitComponent>().Get(entityCaster);
            var irrevocPool = world.GetPool<IrrevocabilityComponent>();
            if (!irrevocPool.Has(entityCaster)) irrevocPool.Add(entityCaster);
            telegoUnitComp.TelegraphingUnitMB.MegaTelegraphingDanger();
        }

        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {

            ref var telegoUnitComp = ref world.GetPool<TelegraphingUnitComponent>().Get(entityCaster);
            telegoUnitComp.TelegraphingUnitMB.DeactiveTeleGO();
            world.GetPool<IrrevocabilityComponent>().Del(entityCaster);
        }
    }
}