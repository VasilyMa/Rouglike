using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class BossObserverSystem : MainEcsSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<BossObserverComponent, BossComponent>> _filter = default;
        readonly EcsPoolInject<BossObserverComponent> _observePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<BossComponent> _bossPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool = default;

        public override MainEcsSystem Clone()
        {
            return new BossObserverSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var healthComp = ref _healthPool.Value.Get(entity);
                ref var observerComp = ref _observePool.Value.Get(entity);
                ref var bossComp = ref _bossPool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);

                observerComp.BossHealthValue.Value = new HealthValue(healthComp.CurrentValue, healthComp.MaxValue);
                observerComp.BossStageValue.Value = new BossStageValue(bossComp.CurrentStage, bossComp.StageCount);

                Vector2 barPosition = new Vector2(0,0);  // TODO on screen?

                observerComp.BossPositionBarValue.Value = barPosition;

                if (_toughnessPool.Value.Has(entity))
                {
                    ref var toughnessComp = ref _toughnessPool.Value.Get(entity);
                    observerComp.BossToughnessbarValue.Value = new ToughnessValue(toughnessComp.CurrentValue, toughnessComp.MaxValueToughness);
                }
            }
        }
    }
}