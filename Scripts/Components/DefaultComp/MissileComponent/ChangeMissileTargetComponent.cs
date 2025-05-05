using AbilitySystem;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;

namespace Client {
    struct ChangeMissileTargetComponent : IAbilityMissileComponent
    {
        [HideIf("ÑlosestEnemy")] public bool RandomEnemy;
        [HideIf("RandomEnemy")] public bool ÑlosestEnemy;
        public bool UniformDistribution;
        public float Range;
        public void Invoke(int entity, EcsWorld world, float charge)
        {
            ref var changeMissileComp = ref world.GetPool<ChangeMissileTargetComponent>().Add(entity);
            changeMissileComp.RandomEnemy = RandomEnemy;
            changeMissileComp.Range = Range;
            changeMissileComp.ÑlosestEnemy = ÑlosestEnemy;
            changeMissileComp.UniformDistribution = UniformDistribution;
        }
    }
}