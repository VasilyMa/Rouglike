using AbilitySystem;
using Leopotam.EcsLite;

namespace Client {
    struct ApprovedInvokeInHitComponent : IAbilityBaseComponent
    {
        public void Init(int entity, EcsWorld world)
        {
            world.GetPool<ApprovedInvokeInHitComponent>().Add(entity);
        }
    }
}