using AbilitySystem;

using Leopotam.EcsLite;

namespace Client 
{
    struct TelegraphCastComponent : IAbilityComponent
    {
        public void Init()
        {

        }

        public void Invoke(int entityCaster, int abilityEntity, EcsWorld world, float charge = 1)
        {
            ref var telegoUnitComp = ref world.GetPool<TelegraphingUnitComponent>().Get(entityCaster);
            telegoUnitComp.TelegraphingUnitMB.TelegraphingDanger();
        }

        public void Dispose(int entityCaster, int abilityEntity,EcsWorld world)
        {
            ref var telegoUnitComp = ref world.GetPool<TelegraphingUnitComponent>().Get(entityCaster);
            telegoUnitComp.TelegraphingUnitMB.DeactiveTeleGO();
        }
    }
}