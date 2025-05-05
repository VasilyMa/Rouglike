using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client 
{
    sealed class EnemyObserverSystem : MainEcsSystem
    {
        readonly EcsFilterInject<Inc<EnemyObserverComponent, EnemyComponent>> _filter = default;
        readonly EcsPoolInject<EnemyObserverComponent> _observePool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ToughnessComponent> _toughnessPool = default;
        readonly EcsPoolInject<TransformComponent> _transformPool = default;

        public override MainEcsSystem Clone()
        {
            return new EnemyObserverSystem();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var healthComp = ref _healthPool.Value.Get(entity);
                ref var observerComp = ref _observePool.Value.Get(entity);
                ref var transformComp = ref _transformPool.Value.Get(entity);

                observerComp.EnemyHealthbarValue.Value = new HealthValue(healthComp.CurrentValue, healthComp.MaxValue);

                Vector2 barPosition = Camera.main.WorldToScreenPoint(transformComp.Transform.position);

                observerComp.EnemyPositionBarValue.Value = barPosition;

                if (_toughnessPool.Value.Has(entity))
                {
                    ref var toughnessComp = ref _toughnessPool.Value.Get(entity);
                    observerComp.EnemyToughnessbarValue.Value = new ToughnessValue(toughnessComp.CurrentValue, toughnessComp.MaxValueToughness);
                }
            }
        }
    }
}