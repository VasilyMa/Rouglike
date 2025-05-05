using AbilitySystem;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Client {
    struct ChangeViewMissileComponent : IAbilityMissileComponent
    {
        public MissileMB newMissile;
        public float Delay;
        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var chargeViewMissileComponent = ref world.GetPool<ChangeViewMissileComponent>().Add(entity);
            chargeViewMissileComponent.newMissile = newMissile;
            chargeViewMissileComponent.Delay = Delay;
        }
    }
}