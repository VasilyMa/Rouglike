using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.Video;

namespace Client {
    sealed class UpdateSelfContextSystem : MainEcsSystem {
        readonly private EcsFilterInject<Inc<UnitBrain, SelfContext, HealthComponent>> _filter = default;
        readonly private EcsPoolInject<SelfContext> _selfContextPool = default;
        readonly private EcsPoolInject<HealthComponent> _healthPool = default;

        public override MainEcsSystem Clone()
        {
            return new UpdateSelfContextSystem();
        }

        public override void Run (IEcsSystems systems) 
        {
            foreach (int entity in _filter.Value)
            {
                ref var selfContext = ref _selfContextPool.Value.Get(entity);
                ref var healthComp = ref _healthPool.Value.Get(entity);
                selfContext.healthPercentage = Mathf.Clamp01(healthComp.CurrentValue / healthComp.MaxValue);
            }
        }
    }
}