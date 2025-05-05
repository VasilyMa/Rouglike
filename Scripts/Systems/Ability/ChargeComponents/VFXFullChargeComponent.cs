using AbilitySystem;
using Leopotam.EcsLite;

namespace Client {
    struct VFXFullChargeComponent : IFullChargeComponent
    {
        public void Invoke(int ownerEntity, EcsWorld world)
        {
            ref var comp = ref world.GetPool<VFXFullChargeComponent>().Add(ownerEntity);
        }
    }
}